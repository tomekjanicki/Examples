using Code.Infrastructure.MessagePack.Formatters.Abstractions;
using Code.StronglyTypeIds;

namespace Code.Infrastructure.MessagePack.Formatters;

public sealed class DeviceIdMessagePackFormatter : Int32BasedMessagePackFormatter<DeviceId>
{
    public DeviceIdMessagePackFormatter() 
        : base(static i => DeviceId.TryCreate(i), static id => id)
    {
    }
}