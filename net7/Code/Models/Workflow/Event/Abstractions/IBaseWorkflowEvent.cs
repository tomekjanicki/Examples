namespace Code.Models.Workflow.Event.Abstractions;

public interface IBaseWorkflowEvent
{
    DateTime Timestamp { get; }

    ProcessTypeWithRequestId Id { get; }
}