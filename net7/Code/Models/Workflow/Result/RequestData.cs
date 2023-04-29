using Code.Models.Workflow.Entities;
using Code.Models.Workflow.Enums;
using Code.Models.Workflow.Result.Enums;
using Code.Types.Collections;

namespace Code.Models.Workflow.Result;

public sealed class RequestData
{
    public RequestStatus RequestStatus { get; private init; }

    public MessageId? MessageId { get; private init; }

    public ProcessTypeWithRequestId? Parent { get; private init; }

    public ReadOnlyArray<Event> Events { get; private init; } = null!;

    public DateTime CreateDateTime { get; private init; }

    public DateTime? StartDateTime { get; private init; }

    public DateTime? CompleteDateTime { get; private init; }

    public string Path { get; private init; } = string.Empty;

    public const string PathSeparator = " => ";

    private readonly MessageId? _slaGroupId;

    public MessageId? GetSlaGroupId() => _slaGroupId;

    private RequestData(MessageId? slaGroupId) => _slaGroupId = slaGroupId;

    public static RequestData GetWaiting(DateTime createDateTime, MessageId? messageId, ProcessTypeWithRequestId? parent, NonEmptyReadOnlyUniqueCollection<ProcessType> path, MessageId? slaGroupId) =>
        new(slaGroupId)
        {
            CreateDateTime = createDateTime,
            RequestStatus = RequestStatus.Waiting,
            Events = ReadOnlyArray<Event>.Empty(),
            MessageId = messageId,
            Parent = parent,
            Path = string.Join(PathSeparator, path)
        };

    public static RequestData GetInProgress(DateTime createDateTime, DateTime startDateTime, ReadOnlyArray<Event> events, MessageId? messageId,
        ProcessTypeWithRequestId? parent, NonEmptyReadOnlyUniqueCollection<ProcessType> path, MessageId? slaGroupId) =>
        new(slaGroupId)
        {
            CreateDateTime = createDateTime,
            StartDateTime = startDateTime,
            RequestStatus = RequestStatus.InProgress,
            Events = events,
            MessageId = messageId,
            Parent = parent,
            Path = string.Join(PathSeparator, path)
        };

    public static RequestData GetComplete(DateTime createDateTime, DateTime startDateTime, DateTime completeDateTime, ReadOnlyArray<Event> events, MessageId? messageId,
        ProcessTypeWithRequestId? parent, NonEmptyReadOnlyUniqueCollection<ProcessType> path, MessageId? slaGroupId) =>
        new(slaGroupId)
        {
            CreateDateTime = createDateTime,
            StartDateTime = startDateTime,
            CompleteDateTime = completeDateTime,
            RequestStatus = RequestStatus.Completed,
            Events = events,
            MessageId = messageId,
            Parent = parent,
            Path = string.Join(PathSeparator, path)
        };
}