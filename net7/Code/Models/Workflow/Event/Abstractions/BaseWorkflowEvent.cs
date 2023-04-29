using Code.Models.Workflow.Entities;

namespace Code.Models.Workflow.Event.Abstractions;

public abstract record BaseWorkflowEvent(DateTime Timestamp, ProcessTypeWithRequestId Id, WorkflowId WorkflowId) : IBaseWorkflowEvent;