using Code.Infrastructure.MessagePack.Formatters.Abstractions;
using Code.StronglyTypeIds;

namespace Code.Infrastructure.MessagePack.Formatters;

public sealed class EMailMessagePackFormatter : StringBasedMessagePackFormatter<EMail>
{
    public EMailMessagePackFormatter() 
        : base(static s => EMail.TryCreate(s), static mail => mail)
    {
    }
}