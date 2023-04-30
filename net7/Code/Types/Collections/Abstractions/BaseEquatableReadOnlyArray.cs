namespace Code.Types.Collections.Abstractions;

public abstract class BaseEquatableReadOnlyArray<T> : BaseReadOnlyArray<T>
{
    protected BaseEquatableReadOnlyArray(IEnumerable<T> enumerable) 
        : base(enumerable)
    {
    }

    protected BaseEquatableReadOnlyArray(T[] array) 
        : base(array)
    {
    }

    protected BaseEquatableReadOnlyArray(T item) 
        : base(item)
    {
    }

    public override bool Equals(object? obj) => EqualsInt(obj);

    public override int GetHashCode() => GetHashCodeInt();

    protected abstract int GetHashCodeInt();

    protected abstract bool EqualsInt(object? obj);

    public static bool operator !=(BaseEquatableReadOnlyArray<T> left, BaseEquatableReadOnlyArray<T> right) => !(left == right);

    public static bool operator ==(BaseEquatableReadOnlyArray<T> left, BaseEquatableReadOnlyArray<T> right) => left.Equals(right);
}