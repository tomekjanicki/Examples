using System.Collections;

namespace Code.Types.Collections.Abstractions;

public abstract class BaseReadOnlyDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    where TKey : notnull
{
    protected Dictionary<TKey, TValue> Dictionary { get; }

    protected BaseReadOnlyDictionary(Dictionary<TKey, TValue> dictionary) => Dictionary = dictionary;

    public Enumerator GetEnumerator() => new(this);

    public struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>
    {
        private Dictionary<TKey, TValue>.Enumerator _enumerator;

        public Enumerator(BaseReadOnlyDictionary<TKey, TValue> set) => _enumerator = set.Dictionary.GetEnumerator();

        public bool MoveNext() => _enumerator.MoveNext();

        public KeyValuePair<TKey, TValue> Current => _enumerator.Current;

        public void Dispose()
        {
        }

        object IEnumerator.Current => _enumerator.Current;

        void IEnumerator.Reset() => ((IEnumerator)_enumerator).Reset();
    }

    IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() => GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public int Count => Dictionary.Count;

    public bool ContainsKey(TKey key) => Dictionary.ContainsKey(key);

    public bool TryGetValue(TKey key, out TValue value) => Dictionary.TryGetValue(key, out value!);

    public TValue this[TKey key] => Dictionary[key];

    IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => Dictionary.Keys;

    IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => Dictionary.Values;

    public Dictionary<TKey, TValue>.KeyCollection KeysAsKeyCollection => Dictionary.Keys;

    public Dictionary<TKey, TValue>.ValueCollection ValuesAsValueCollection => Dictionary.Values;

    public TValue GetValueOrDefault(TKey key, TValue defaultValue) => Dictionary.TryGetValue(key, out var value) ? value : defaultValue;

    protected static bool AreEqual(BaseReadOnlyDictionary<TKey, TValue> me, BaseReadOnlyDictionary<TKey, TValue> oth)
    {
        if (me.Count != oth.Count)
        {
            return false;
        }
        foreach (var keyValuePair in me)
        {
            var meKey = keyValuePair.Key;
            if (!oth.TryGetValue(meKey, out var value))
            {
                return false;
            }
            var meValue = keyValuePair.Value;
            if (value is null && meValue is not null || value is not null && meValue is null)
            {
                return false;
            }
            if (value is not null && meValue is not null && !meValue.Equals(value))
            {
                return false;
            }
        }

        return true;
    }
}