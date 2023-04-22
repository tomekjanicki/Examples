using MessagePack;
using MessagePack.Formatters;
using OneOf;
using OneOf.Types;

namespace Code.Infrastructure.MessagePack.Formatters.Abstractions;

public abstract class StructBasedMessagePackFormatter<T, TPrimitiveType> : IMessagePackFormatter<T>
    where T : struct
    where TPrimitiveType : struct
{
    private readonly Func<TPrimitiveType?, OneOf<T, Error<string>>> _constructorFactory;
    private readonly Func<T, TPrimitiveType> _valueProvider;

    protected StructBasedMessagePackFormatter(Func<TPrimitiveType?, OneOf<T, Error<string>>> constructorFactory, Func<T, TPrimitiveType> valueProvider)
    {
        _constructorFactory = constructorFactory;
        _valueProvider = valueProvider;
    }

    public void Serialize(ref MessagePackWriter writer, T value, MessagePackSerializerOptions options) => options.Resolver.GetFormatterWithVerify<TPrimitiveType>().Serialize(ref writer, _valueProvider(value), options);

    public T Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
        var value = options.Resolver.GetFormatterWithVerify<TPrimitiveType?>().Deserialize(ref reader, options);
        var createResult = _constructorFactory(value);
        if (createResult.IsT0)
        {
            return createResult.AsT0;
        }

        throw new MessagePackSerializationException(createResult.AsT1.Value);
    }
}