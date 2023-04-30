using Code.Models.Workflow;
using Code.Models.Workflow.Entities;
using Code.Models.Workflow.Enums;
using Code.Models.Workflow.Event.Abstractions;
using Code.Models.Workflow.Result;
using Code.Models.Workflow.Result.Enums;
using Code.Types.Collections;

namespace Code;

public static class Functional
{
    public static Workflow GetWorkflow(WorkflowId id,
        NonEmptyReadOnlyDictionary<ProcessTypeWithRequestId, RequestData> requests,
        ReadOnlyDictionary<MessageId, SlaData> slas,
        NonEmptyReadOnlyDictionary<MessageId, MessageData> messages) =>
        new()
        {
            Id = id,
            Requests = requests,
            Completed = requests.All(pair => pair.Value.RequestStatus == RequestStatus.Completed),
            Messages = messages,
            Slas = slas
        };

    private static readonly NonEmptyReadOnlyUniqueCollection<ProcessType> NonSlaProcessTypes = NonEmptyReadOnlyUniqueCollection<ProcessType>.TryCreate(new[] { ProcessType.ColdStorePublisher, ProcessType.ColdStoreLoader }).AsT0;

    public static ReadOnlyDictionary<Guid, SlaData> GetSlas(NonEmptyReadOnlyDictionary<ProcessTypeWithRequestId, RequestData> sourceRequests,
        ReadOnlyDictionary<Guid, NonEmptyReadOnlyArray<byte>> slaHashes)
    {
        var requestsGroupedBySlaGroupId = sourceRequests
            .Where(static pair => pair.Value.GetSlaGroupId() is not null && !NonSlaProcessTypes.Contains(pair.Key.ProcessType))
            .GroupBy(static pair => pair.Value.GetSlaGroupId()!.Value)
            .Select(static pairs => (GroupId: pairs.Key, Requests: pairs.Select(pair => pair.Value)));
        var results = new Dictionary<Guid, SlaData>();
        foreach (var groupedRequests in requestsGroupedBySlaGroupId)
        {
            var requests = groupedRequests.Requests.ToArray();
            if (requests.Any(static pair => pair.RequestStatus != RequestStatus.Completed))
            {
                continue;
            }
            var startRequestDateTime = requests.Single(pair => pair.MessageId is not null && pair.MessageId.Value == groupedRequests.GroupId).CreateDateTime;
            var endRequestDate = requests.Select(static pair => pair.CompleteDateTime!.Value).Max();
            var persistActionAndHash = GetPersistActionAndHash(slaHashes, groupedRequests.GroupId, startRequestDateTime, endRequestDate);

            results.Add(groupedRequests.GroupId, new SlaData { EndDate = endRequestDate, StartDate = startRequestDateTime, Action = persistActionAndHash.Action, Hash = persistActionAndHash.Hash });
        }

        return new ReadOnlyDictionary<Guid, SlaData>(results);
    }

    private sealed record PersistActionAndHash(PersistAction Action, NonEmptyReadOnlyArray<byte> Hash);

    private static PersistActionAndHash GetPersistActionAndHash(ReadOnlyDictionary<Guid, NonEmptyReadOnlyArray<byte>> slaHashes, Guid groupId, DateTime startRequestDateTime, DateTime endRequestDate)
    {
        var currentHash = HashCalculator.GetHash(startRequestDateTime, endRequestDate);
        return !slaHashes.TryGetValue(groupId, out var hash)
            ? new PersistActionAndHash(PersistAction.Insert, currentHash)
            : new PersistActionAndHash(currentHash != hash ? PersistAction.Update : PersistAction.None, currentHash);
    }

    public static NonEmptyReadOnlyDictionary<MessageId, MessageData> GetMessages(NonEmptyReadOnlyArray<IBaseWorkflowEvent> events)
    {
        var messages = events.OfType<IWithMessageEvent>().Select(static @event => @event.Message);
        var results = messages.ToDictionary(static message => message.Id, static message => new MessageData { MessageType = message.Type, Metadata = message.Metadata });

        return NonEmptyReadOnlyDictionary<MessageId, MessageData>.TryCreate(results).AsT0;
    }

    public static NonEmptyReadOnlyDictionary<ProcessTypeWithRequestId, RequestData> GetRequests(NonEmptyReadOnlyArray<IBaseWorkflowEvent> events,
        NonEmptyReadOnlyDictionary<MessageType, NonEmptyReadOnlyArray<ProcessType>> processTypesForMessageTypes)
    {
        var results = new Dictionary<ProcessTypeWithRequestId, RequestData>();
        var initial = events.OfType<IInitialEvent>().Single();
        results.Add(initial.Id, GetRequestData(initial));
        var path = new HashSet<ProcessType> { initial.Id.ProcessType };
        FillRequestDictionary(initial.Message.Id, initial.Message.Type, initial.Id.RequestId, initial.Timestamp, initial.Id, path, events, processTypesForMessageTypes, results,
            initial.Message.GetSlaGroupId());

        return NonEmptyReadOnlyDictionary<ProcessTypeWithRequestId, RequestData>.TryCreate(results).AsT0;
    }

    public static RequestData GetRequestData(IInitialEvent initialEvent) =>
        RequestData.GetComplete(initialEvent.Timestamp, initialEvent.Timestamp, initialEvent.Timestamp,
            new ReadOnlyArray<Event>(new[] { new Event { Timestamp = initialEvent.Timestamp, Type = initialEvent.GetEventType() } }), null, null,
            new NonEmptyReadOnlyUniqueCollection<ProcessType>(initialEvent.Id.ProcessType), initialEvent.Message.GetSlaGroupId());

    private static MessageId? GetSlaGroupId(this Message message) => message.Type != MessageType.EnrichmentMessage ? null : message.Id;

    public static RequestData GetRequestData(ReadOnlyArray<IBaseWorkflowEvent> eventsForProcess, DateTime createDateTime, MessageId messageId, ProcessTypeWithRequestId parentId,
        NonEmptyReadOnlyUniqueCollection<ProcessType> path, MessageId? slaGroupId)
    {
        if (eventsForProcess.Count == 0)
        {
            return RequestData.GetWaiting(createDateTime, messageId, parentId, path, slaGroupId);
        }
        var events = new ReadOnlyArray<Event>(eventsForProcess.Select(@event => new Event { Timestamp = @event.Timestamp, Type = @event.GetEventType() }).ToArray());
        var startEvent = eventsForProcess.OfType<IStartEvent>().Single();
        var endEvent = eventsForProcess.OfType<IEndEvent>().SingleOrDefault();

        return endEvent is not null
            ?
            RequestData.GetComplete(createDateTime, startEvent.Timestamp, endEvent.Timestamp, events, messageId, parentId, path, slaGroupId)
            :
            RequestData.GetInProgress(createDateTime, startEvent.Timestamp, events, messageId, parentId, path, slaGroupId);
    }

    private static void FillRequestDictionary(MessageId messageId, MessageType messageType, RequestId newRequestId, DateTime timestamp, ProcessTypeWithRequestId parentId,
        HashSet<ProcessType> path,
        NonEmptyReadOnlyArray<IBaseWorkflowEvent> events,
        NonEmptyReadOnlyDictionary<MessageType, NonEmptyReadOnlyArray<ProcessType>> processTypesForMessageTypes,
        Dictionary<ProcessTypeWithRequestId, RequestData> results, MessageId? slaGroupId)
    {
        var childProcessTypes = processTypesForMessageTypes[messageType];
        if (messageType == MessageType.EnrichmentMessage)
        {
            slaGroupId = messageId;
        }
        foreach (var childProcessType in childProcessTypes)
        {
            path.Add(childProcessType);
            var id = new ProcessTypeWithRequestId(childProcessType, newRequestId);
            var childEvents = new ReadOnlyArray<IBaseWorkflowEvent>(events.Where(@event => @event.Id == id).ToArray());
            results.Add(id, GetRequestData(childEvents, timestamp, messageId, parentId, NonEmptyReadOnlyUniqueCollection<ProcessType>.TryCreate(path).AsT0, slaGroupId));
            var triggers = childEvents.OfType<ITriggerEvent>();
            foreach (var trigger in triggers)
            {
                FillRequestDictionary(trigger.Message.Id, trigger.Message.Type, trigger.NewRequestId, trigger.Timestamp, trigger.Id, path, events, processTypesForMessageTypes, results, slaGroupId);
            }
        }
    }

    public static EventType GetEventType(this IBaseWorkflowEvent workflowEvent) =>
        workflowEvent switch
        {
            IInitialEvent => EventType.Initial,
            IStartEvent => EventType.Start,
            ITriggerEvent => EventType.Trigger,
            IEndEvent => EventType.End,
            _ => throw new ArgumentOutOfRangeException(nameof(workflowEvent))
        };
}