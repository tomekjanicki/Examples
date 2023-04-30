namespace Code.Types.Collections.Abstractions;

public abstract class BaseEquatableReadOnlyDictionary<TKey, TValue> : BaseReadOnlyDictionary<TKey, TValue>
    where TKey : notnull
{
    protected BaseEquatableReadOnlyDictionary(Dictionary<TKey, TValue> dictionary) 
        : base(dictionary)
    {
    }

    public override bool Equals(object? obj) => EqualsInt(obj);

    public override int GetHashCode() => GetHashCodeInt();

    protected abstract int GetHashCodeInt();

    protected abstract bool EqualsInt(object? obj);

    public static bool operator !=(BaseEquatableReadOnlyDictionary<TKey, TValue> left, BaseEquatableReadOnlyDictionary<TKey, TValue> right) => !(left == right);

    public static bool operator ==(BaseEquatableReadOnlyDictionary<TKey, TValue> left, BaseEquatableReadOnlyDictionary<TKey, TValue> right) => left.Equals(right);
}