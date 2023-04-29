using System.Collections;

namespace Code.Types.Collections.Abstractions;

public abstract class BaseReadOnlyUniqueCollection<T> : IReadOnlyCollection<T>
{
    private readonly HashSet<T> _hashSet;

    protected BaseReadOnlyUniqueCollection(IEnumerable<T> items) => _hashSet = new HashSet<T>(items);

    protected BaseReadOnlyUniqueCollection(T item) => _hashSet = new HashSet<T> { item };

    public Enumerator GetEnumerator() => new(this);

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public int Count => _hashSet.Count;

    public bool Contains(T item) => _hashSet.Contains(item);

    public bool IsEquivalent(BaseReadOnlyUniqueCollection<T> values) => _hashSet.SetEquals(values._hashSet);

    public struct Enumerator : IEnumerator<T>
    {
        private HashSet<T>.Enumerator _enumerator;

        public Enumerator(BaseReadOnlyUniqueCollection<T> set) => _enumerator = set._hashSet.GetEnumerator();

        public bool MoveNext() => _enumerator.MoveNext();

        public T Current => _enumerator.Current;

        public void Dispose()
        {
        }

        object? IEnumerator.Current => _enumerator.Current;

        void IEnumerator.Reset() => ((IEnumerator)_enumerator).Reset();
    }
}