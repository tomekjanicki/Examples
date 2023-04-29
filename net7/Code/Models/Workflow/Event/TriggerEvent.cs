using Code.Models.Workflow.Entities;
using Code.Models.Workflow.Event.Abstractions;

namespace Code.Models.Workflow.Event;

public sealed record TriggerEvent(DateTime Timestamp, ProcessTypeWithRequestId Id, WorkflowId WorkflowId, RequestId NewRequestId, Message Message) : BaseWorkflowEvent(Timestamp, Id, WorkflowId), ITriggerEvent;