using Code.Models.Workflow.Enums;
using Code.Models.Workflow.Event.Abstractions;
using Code.Models.Workflow.Event;
using Code.Models.Workflow.Result.Enums;
using Code.Models.Workflow.Result;
using Code.Models.Workflow;
using Code.Types.Collections;
using Code;
using Code.Models.Workflow.Entities;
using FluentAssertions;

namespace Test;

public class FunctionalTests
{
    [Fact]
    public void GetRequestsShouldWorkAsExpected()
    {
        var workflowId = new WorkflowId(Guid.NewGuid());
        var requestId1 = new RequestId(Guid.NewGuid());
        var requestId2 = new RequestId(Guid.NewGuid());
        var idRdp = new ProcessTypeWithRequestId(ProcessType.ReportDataProvider, requestId1);
        var idE = new ProcessTypeWithRequestId(ProcessType.Enrichment, requestId1);
        var idV = new ProcessTypeWithRequestId(ProcessType.Validation, requestId2);
        var rdpMessage = new Message((MessageId)Guid.NewGuid(), MessageType.ReportDataProviderMessage, null);
        var eMessage = new Message((MessageId)Guid.NewGuid(), MessageType.EnrichmentMessage, null);
        var events = new IBaseWorkflowEvent[]
        {
            new InitialEvent(new DateTime(2020, 1, 1, 10, 0, 0), idRdp, workflowId, rdpMessage),

            new StartEvent(new DateTime(2020, 1, 1, 10, 5, 0), idE, workflowId),
            new TriggerEvent(new DateTime(2020, 1, 1, 10, 10, 0), idE, workflowId, requestId2, eMessage),
            new EndEvent(new DateTime(2020, 1, 1, 10, 15, 0), idE, workflowId)
        };
        var nonEmptyReadOnlyArray = new NonEmptyReadOnlyArray<IBaseWorkflowEvent>(events);
        var requests = Functional.GetRequests(nonEmptyReadOnlyArray, Configuration.ProcessTypesForMessageTypes);

        AssertRequest(requests[idRdp], RequestStatus.Completed, 1, null, "ReportDataProvider");
        AssertRequest(requests[idE], RequestStatus.Completed, 3, idRdp, "ReportDataProvider => Enrichment");
        AssertRequest(requests[idV], RequestStatus.Waiting, 0, idE, "ReportDataProvider => Enrichment => Validation");
    }

    [Fact]
    public void GetSlasShouldWorkAsExpected()
    {
        var externalMessageId = new MessageId(Guid.NewGuid());
        var startDateTime = new DateTime(2020, 1, 1, 10, 0, 0);
        var endDateTime = new DateTime(2020, 1, 1, 11, 0, 0);
        var requests = new Dictionary<ProcessTypeWithRequestId, RequestData>
        {
            { new ProcessTypeWithRequestId(ProcessType.Enrichment, (RequestId)Guid.NewGuid()), RequestData.GetComplete(startDateTime,
                new DateTime(2020, 1, 1, 10, 10, 0), new DateTime(2020, 1, 1, 10, 11, 0), ReadOnlyArray<Event>.Empty(), externalMessageId,
                null, new NonEmptyReadOnlyUniqueCollection<ProcessType>(ProcessType.Enrichment), externalMessageId)},
            { new ProcessTypeWithRequestId(ProcessType.Validation, (RequestId)Guid.NewGuid()), RequestData.GetComplete(new DateTime(2020, 1, 1, 10, 12, 0),
                new DateTime(2020, 1, 1, 10, 12, 0), endDateTime, ReadOnlyArray<Event>.Empty(), (MessageId)Guid.NewGuid(),
                null, new NonEmptyReadOnlyUniqueCollection<ProcessType>(ProcessType.Validation), externalMessageId)},
            { new ProcessTypeWithRequestId(ProcessType.ColdStorePublisher, (RequestId)Guid.NewGuid()), RequestData.GetComplete(new DateTime(2020, 1, 1, 10, 12, 0),
                new DateTime(2020, 1, 1, 10, 12, 0), new DateTime(2020, 1, 1, 11, 10, 0), ReadOnlyArray<Event>.Empty(), (MessageId)Guid.NewGuid(),
                null, new NonEmptyReadOnlyUniqueCollection<ProcessType>(ProcessType.ColdStorePublisher), externalMessageId)}
        };
        var slas = Functional.GetSlas(new NonEmptyReadOnlyDictionary<ProcessTypeWithRequestId, RequestData>(requests),
            ReadOnlyDictionary<Guid, NonEmptyReadOnlyArray<byte>>.Empty());
        slas.Count.Should().Be(1);
        var sla = slas[externalMessageId];
        sla.StartDate.Should().Be(startDateTime);
        sla.EndDate.Should().Be(endDateTime);
    }

    private static void AssertRequest(RequestData request, RequestStatus requestStatus, int eventsCount, ProcessTypeWithRequestId? parent,
        string path)
    {
        request.RequestStatus.Should().Be(requestStatus);
        request.Events.Count.Should().Be(eventsCount);
        switch (parent)
        {
            case null:
                request.Parent.Should().BeNull();
                break;
            default:
                request.Parent.Should().Be(parent.Value);
                break;
        }
        request.Path.Should().Be(path);
    }
}