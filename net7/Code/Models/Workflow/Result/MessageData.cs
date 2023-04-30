using Code.Models.Workflow.Enums;
using Code.Models.Workflow.MessageMetadata;

namespace Code.Models.Workflow.Result;

public sealed class MessageData
{
    public required MessageType MessageType { get; init; }

    public required Root? Metadata { get; init; }
}