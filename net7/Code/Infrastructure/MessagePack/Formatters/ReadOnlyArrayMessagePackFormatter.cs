using Code.Types.Collections;
using MessagePack;
using MessagePack.Formatters;

namespace Code.Infrastructure.MessagePack.Formatters;

public sealed class ReadOnlyArrayMessagePackFormatter<T> : IMessagePackFormatter<ReadOnlyArray<T>>
{
    public void Serialize(ref MessagePackWriter writer, ReadOnlyArray<T>? value, MessagePackSerializerOptions options)
        => MessagePackSerializationHelper.SerializeArray(ref writer, value, options);

    public ReadOnlyArray<T> Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
        => MessagePackSerializationHelper.DeserializeArray<T, ReadOnlyArray<T>>(ref reader, options,
            static arg => new ReadOnlyArray<T>(arg), static () => ReadOnlyArray<T>.Empty());
}