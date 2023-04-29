using Code.Types.Collections;
using MessagePack;
using MessagePack.Formatters;

namespace Code.Infrastructure.MessagePack.Formatters;

public sealed class ReadOnlyDictionaryMessagePackFormatter<TKey, TValue> : IMessagePackFormatter<ReadOnlyDictionary<TKey, TValue>>
    where TKey : notnull
{
    public void Serialize(ref MessagePackWriter writer, ReadOnlyDictionary<TKey, TValue>? value, MessagePackSerializerOptions options)
        => MessagePackSerializationHelper.SerializeDictionary(ref writer, value, options);

    public ReadOnlyDictionary<TKey, TValue> Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
        => MessagePackSerializationHelper.DeserializeDictionary<TKey, TValue, ReadOnlyDictionary<TKey, TValue>>(ref reader, options,
            static arg => new ReadOnlyDictionary<TKey, TValue>(arg), static () => ReadOnlyDictionary<TKey, TValue>.Empty());
}