using Code.Models.Workflow.Entities;
using Code.Models.Workflow.Event.Abstractions;

namespace Code.Models.Workflow.Event;

public sealed record EndEvent(DateTime Timestamp, ProcessTypeWithRequestId Id, WorkflowId WorkflowId) : BaseWorkflowEvent(Timestamp, Id, WorkflowId), IEndEvent;