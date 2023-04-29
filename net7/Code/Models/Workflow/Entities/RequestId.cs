namespace Code.Models.Workflow.Entities;

public readonly record struct RequestId(Guid Value)
{
    public RequestId()
        : this(Guid.Empty)
    {
    }

    public static implicit operator Guid(RequestId requestId) => requestId.Value;

    public static explicit operator RequestId(Guid value) => new(value);
}