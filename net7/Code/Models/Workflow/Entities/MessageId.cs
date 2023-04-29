namespace Code.Models.Workflow.Entities;

public readonly record struct MessageId(Guid Value)
{
    public MessageId() 
        : this(Guid.Empty)
    {
    }

    public static implicit operator Guid(MessageId messageId) => messageId.Value;

    public static explicit operator MessageId(Guid value) => new(value);
}