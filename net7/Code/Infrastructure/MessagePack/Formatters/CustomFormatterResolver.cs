using MessagePack.Formatters;
using MessagePack;

namespace Code.Infrastructure.MessagePack.Formatters;

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
            var formatter = CustomFormatterResolverGetHelper.GetFormatter(typeof(T));
            if (formatter is not null)
            {
                Formatter = (IMessagePackFormatter<T>)formatter;
            }
        }
    }
}