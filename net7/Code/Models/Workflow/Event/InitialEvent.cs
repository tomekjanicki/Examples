using Code.Models.Workflow.Entities;
using Code.Models.Workflow.Event.Abstractions;

namespace Code.Models.Workflow.Event;

public sealed record InitialEvent(DateTime Timestamp, ProcessTypeWithRequestId Id, WorkflowId WorkflowId, Message Message) : BaseWorkflowEvent(Timestamp, Id, WorkflowId), IInitialEvent;