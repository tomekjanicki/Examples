using MessagePack;
using MessagePack.Formatters;

namespace Code.Infrastructure.MessagePack.Formatters;

public sealed class ReadOnlyCollectionMessagePackFormatter<T> : IMessagePackFormatter<IReadOnlyCollection<T>>
{
    public void Serialize(ref MessagePackWriter writer, IReadOnlyCollection<T>? value, MessagePackSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNil();

            return;
        }
        options.Resolver.GetFormatterWithVerify<IEnumerable<T>>().Serialize(ref writer, value, options);
    }

    public IReadOnlyCollection<T> Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
        => reader.IsNil ? Array.Empty<T>() : options.Resolver.GetFormatterWithVerify<T[]>().Deserialize(ref reader, options);
}