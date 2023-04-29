using Code.Models.Workflow.Entities.Abstractions;
using OneOf;
using OneOf.Types;

namespace Code.Models.Workflow.Entities;

public sealed record ArrayItemKey : ItemKey
{
    private ArrayItemKey(string value, int index)
    {
        Value = value;
        Index = index;
    }

    public string Value { get; }

    public int Index { get; }

    public static OneOf<ArrayItemKey, Error<string>> TryCreate(string? value, int? index)
    {
        if (string.IsNullOrEmpty(value))
        {
            return new Error<string>(NullOrEmptyValueError);
        }
        if (index is null)
        {
            return new Error<string>(NullIndexError);
        }
        return new ArrayItemKey(value, index.Value);
    }

    public const string NullOrEmptyValueError = "Value is null or empty.";
    public const string NullIndexError = "Index is null.";
}