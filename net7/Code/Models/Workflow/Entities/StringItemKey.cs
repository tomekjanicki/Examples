using Code.Models.Workflow.Entities.Abstractions;
using OneOf;
using OneOf.Types;

namespace Code.Models.Workflow.Entities;

public sealed record StringItemKey : ItemKey
{
    private StringItemKey(string value) => Value = value;

    public string Value { get; }

    public static explicit operator StringItemKey(string value) => Extensions.GetValueWhenSuccessOrThrowInvalidCastException(value, static p => TryCreate(p));

    public static implicit operator string(StringItemKey value) => value.Value;

    public static OneOf<StringItemKey, Error<string>> TryCreate(string? value)
        => string.IsNullOrEmpty(value) ? new Error<string>(NullOrEmptyValueError) : new StringItemKey(value);

    public const string NullOrEmptyValueError = "Value is null or empty.";
}