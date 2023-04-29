using System.Collections;

namespace Code.Types.Collections.Abstractions;

public abstract class BaseReadOnlyArray<T> : IReadOnlyList<T>
{
    protected T[] Array { get; }

    protected BaseReadOnlyArray(IEnumerable<T> enumerable) => Array = enumerable.ToArray();

    protected BaseReadOnlyArray(T[] array) => Array = array;

    protected BaseReadOnlyArray(T item) => Array = new[] { item };

    public Enumerator GetEnumerator() => new(this);

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public int Count => Array.Length;

    public T this[int index] => Array[index];

    public struct Enumerator : IEnumerator<T>
    {
        private readonly T[] _array;
        private int _index = -1;

        public Enumerator(BaseReadOnlyArray<T> readOnlyArray) => _array = readOnlyArray.Array;

        public bool MoveNext()
        {
            _index += 1;

            return _index < _array.Length;
        }

        public T Current => _array[_index];

        public void Dispose()
        {
        }

        object? IEnumerator.Current => Current;

        void IEnumerator.Reset() => _index = -1;
    }

    public ReadOnlySpan<T> AsReadOnlySpan() => Array.AsSpan();

    public ReadOnlyMemory<T> ToReadOnlyMemory() => new(Array);
}