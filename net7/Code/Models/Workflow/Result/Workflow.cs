using Code.Models.Workflow.Entities;
using Code.Models.Workflow.Enums;
using Code.Types.Collections;

namespace Code.Models.Workflow.Result;

public sealed class Workflow
{
    public required WorkflowId Id { get; init; }

    public required bool Completed { get; init; }

    public required NonEmptyReadOnlyDictionary<ProcessTypeWithRequestId, RequestData> Requests { get; init; }

    public required NonEmptyReadOnlyDictionary<MessageId, MessageType> Messages { get; init; }

    public required ReadOnlyDictionary<MessageId, SlaData> Slas { get; init; }
}