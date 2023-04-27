using Code.Infrastructure.MessagePack.Formatters;
using MessagePack.Formatters;

namespace Code.Infrastructure.MessagePack.Resolvers;

public static class CustomFormatterResolverGetHelper
{
    private static readonly Dictionary<Type, object> FormatterMap = new()
    {
        {typeof(IReadOnlyCollection<int>), new ReadOnlyCollectionMessagePackFormatter<int>()}
    };

    public static IMessagePackFormatter<T>? GetFormatter<T>() => FormatterMap.TryGetValue(typeof(T), out var formatter) ? (IMessagePackFormatter<T>)formatter : null;
}