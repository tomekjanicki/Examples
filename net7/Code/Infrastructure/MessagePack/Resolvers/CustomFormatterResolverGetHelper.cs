using Code.Infrastructure.MessagePack.Formatters;

namespace Code.Infrastructure.MessagePack.Resolvers;

public static class CustomFormatterResolverGetHelper
{
    private static readonly Dictionary<Type, object> FormatterMap = new()
    {
        {typeof(IReadOnlyCollection<int>), new ReadOnlyCollectionMessagePackFormatter<int>()}
    };

    public static object? GetFormatter(Type t) => FormatterMap.TryGetValue(t, out var formatter) ? formatter : null;
}