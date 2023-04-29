using MessagePack;
using MessagePack.Formatters;

namespace Code.Infrastructure.MessagePack.Formatters;

public sealed class ReadOnlyCollectionMessagePackFormatter<T> : IMessagePackFormatter<IReadOnlyCollection<T>>
{
    public void Serialize(ref MessagePackWriter writer, IReadOnlyCollection<T>? value, MessagePackSerializerOptions options)
        => MessagePackSerializationHelper.SerializeArray(ref writer, value, options);

    public IReadOnlyCollection<T> Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
        => MessagePackSerializationHelper.DeserializeArray<T, IReadOnlyCollection<T>>(ref reader, options,
            static arg => arg, static () => Array.Empty<T>());

}