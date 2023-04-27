using Code.Infrastructure.MessagePack.Resolvers;
using MessagePack;
using MessagePack.Resolvers;

namespace Code.Infrastructure.MessagePack;

public static class MessagePackSerializerWrapper
{
    private static readonly MessagePackSerializerOptions Options;

    static MessagePackSerializerWrapper()
    {
        var resolver = CompositeResolver.Create
        (
            CustomFormatterResolver.Instance,
            StandardResolver.Instance
        );
        Options = MessagePackSerializerOptions.Standard.WithResolver(resolver);
    }

    public static byte[] Serialize(object? obj) => MessagePackSerializer.Serialize(obj, Options);

    public static T Deserialize<T>(byte[] value) => MessagePackSerializer.Deserialize<T>(value, Options);
}