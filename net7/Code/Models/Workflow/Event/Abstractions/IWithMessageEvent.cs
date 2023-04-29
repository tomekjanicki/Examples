namespace Code.Models.Workflow.Event.Abstractions;

public interface IWithMessageEvent : IBaseWorkflowEvent
{
    Message Message { get; }
}