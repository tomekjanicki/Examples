using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Code.Infrastructure.Json.Converters;
using Code.Infrastructure.MessagePack.Formatters;
using MessagePack;
using OneOf;
using OneOf.Types;

namespace Code.StronglyTypeIds;

[JsonConverter(typeof(EMailJsonConverter))]
[MessagePackFormatter(typeof(EMailMessagePackFormatter))]
public sealed record EMail
{
    private static readonly Regex Regex = new(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

    private EMail(string value) => Value = value;

    public string Value { get; }

    public static explicit operator EMail(string value) => Extensions.GetValueWhenSuccessOrThrowInvalidCastException(value, static p => TryCreate(p));

    public static implicit operator string(EMail value) => value.Value;

    public static OneOf<EMail, Error<string>> TryCreate(string? value)
    {
        if (value is null)
        {
            return new Error<string>(NullValueError);
        }
        if (value.Length > 320)
        {
            return new Error<string>(ValueGreaterThan320CharsError);
        }
        var match = Regex.Match(value);

        return match.Success ? new EMail(value) : new Error<string>(ValueNotValidEMailError);
    }

    public const string NullValueError = "Value is null.";
    public const string ValueGreaterThan320CharsError = "Value is greater than 320 chars.";
    public const string ValueNotValidEMailError = "Value is not valid email.";
}