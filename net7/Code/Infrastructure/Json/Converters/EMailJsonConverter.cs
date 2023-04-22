using Code.Infrastructure.Json.Converters.Abstractions;
using Code.StronglyTypeIds;

namespace Code.Infrastructure.Json.Converters;

public sealed class EMailJsonConverter : StringBasedJsonConverter<EMail>
{
    public EMailJsonConverter()
        : base(static s => EMail.TryCreate(s), static mail => mail)
    {
    }
}