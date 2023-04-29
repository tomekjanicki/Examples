using Code.Models.Workflow.Entities;

namespace Code.Models.Workflow.Event.Abstractions;

public interface ITriggerEvent : IWithMessageEvent
{
    RequestId NewRequestId { get; }
}