namespace Code.Models.Workflow.Entities;

public readonly record struct WorkflowId(Guid Value)
{
    public WorkflowId() 
        : this(Guid.Empty)
    {
    }

    public static implicit operator Guid(WorkflowId workflowId) => workflowId.Value;

    public static explicit operator WorkflowId(Guid value) => new(value);
}