using System.Text.Json.Serialization;
using Code.Infrastructure.Json.Converters;
using Code.Infrastructure.MessagePack.Formatters;
using MessagePack;
using OneOf;
using OneOf.Types;

namespace Code.StronglyTypeIds;

[JsonConverter(typeof(DeviceIdJsonConverter))]
[MessagePackFormatter(typeof(DeviceIdMessagePackFormatter))]
public readonly record struct DeviceId
{
    private readonly bool _initialized;
    private readonly int _value;

    public DeviceId()
    {
        _value = 1;
        _initialized = true;
    }

    private DeviceId(int value)
    {
        _value = value;
        _initialized = true;
    }

    public int Value => !_initialized ? throw new InvalidOperationException("Object is not initialized") : _value;

    public static implicit operator int(DeviceId deviceId) => deviceId.Value;

    public static explicit operator DeviceId(int value) => Extensions.GetValueWhenSuccessOrThrowInvalidCastException(value, static p => TryCreate(p));

    public static DeviceId Min => new();

    public static DeviceId Max() => new(int.MaxValue);

    public static OneOf<DeviceId, Error<string>> TryCreate(int? value) =>
        value switch
        {
            null => new Error<string>(NullValueError),
            <= 0 => new Error<string>(ValueNotGreaterThanZeroError),
            _ => new DeviceId(value.Value)
        };

    public const string NullValueError = "Value is null.";
    public const string ValueNotGreaterThanZeroError = "Value has to be grater than zero.";
}