using OneOf;
using OneOf.Types;

namespace Code.Infrastructure.MessagePack.Formatters.Abstractions;

public abstract class Int32BasedMessagePackFormatter<T> : StructBasedMessagePackFormatter<T, int>
    where T : struct
{
    protected Int32BasedMessagePackFormatter(Func<int?, OneOf<T, Error<string>>> constructorFactory, Func<T, int> valueProvider) 
        : base(constructorFactory, valueProvider)
    {
    }
}