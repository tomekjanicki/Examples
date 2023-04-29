using Code.Models.Workflow.Result.Enums;

namespace Code.Models.Workflow.Result;

public sealed class Event
{
    public required DateTime Timestamp { get; init; }

    public required EventType Type { get; init; }
}