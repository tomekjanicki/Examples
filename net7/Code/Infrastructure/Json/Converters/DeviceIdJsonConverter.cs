using Code.Infrastructure.Json.Converters.Abstractions;
using Code.StronglyTypeIds;

namespace Code.Infrastructure.Json.Converters;

public sealed class DeviceIdJsonConverter : Int32BasedJsonConvert<DeviceId>
{
    public DeviceIdJsonConverter()
        : base(static i => DeviceId.TryCreate(i), static id => id)
    {
    }
}