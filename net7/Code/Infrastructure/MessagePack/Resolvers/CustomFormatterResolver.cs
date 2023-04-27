using MessagePack.Formatters;
using MessagePack;

namespace Code.Infrastructure.MessagePack.Resolvers;

public sealed class CustomFormatterResolver : IFormatterResolver
{
    public static readonly IFormatterResolver Instance = new CustomFormatterResolver();

    private CustomFormatterResolver()
    {
    }

    public IMessagePackFormatter<T>? GetFormatter<T>() => FormatterCache<T>.Formatter;

    private static class FormatterCache<T>
    {
        public static readonly IMessagePackFormatter<T>? Formatter;

        static FormatterCache()
        {
            var formatter = CustomFormatterResolverGetHelper.GetFormatter<T>();
            if (formatter is not null)
            {
                Formatter = formatter;
            }
        }
    }
}