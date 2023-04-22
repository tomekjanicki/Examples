using MessagePack;
using MessagePack.Formatters;
using OneOf;
using OneOf.Types;

namespace Code.Infrastructure.MessagePack.Formatters.Abstractions;

public abstract class StringBasedMessagePackFormatter<T> : IMessagePackFormatter<T?>
    where T : class
{
    private readonly Func<string?, OneOf<T, Error<string>>> _constructorFactory;
    private readonly Func<T, string> _valueProvider;

    protected StringBasedMessagePackFormatter(Func<string?, OneOf<T, Error<string>>> constructorFactory, Func<T, string> valueProvider)
    {
        _constructorFactory = constructorFactory;
        _valueProvider = valueProvider;
    }

    public void Serialize(ref MessagePackWriter writer, T? value, MessagePackSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNil();

            return;
        }
        options.Resolver.GetFormatterWithVerify<string>().Serialize(ref writer, _valueProvider(value), options);
    }

    public T? Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
        var value = options.Resolver.GetFormatterWithVerify<string?>().Deserialize(ref reader, options);
        if (value is null)
        {
            return null;
        }
        var createResult = _constructorFactory(value);
        if (createResult.IsT0)
        {
            return createResult.AsT0;
        }

        throw new MessagePackSerializationException(createResult.AsT1.Value);
    }
}