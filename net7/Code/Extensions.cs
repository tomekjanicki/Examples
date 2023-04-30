using OneOf;
using OneOf.Types;

namespace Code;

public static class Extensions
{
    public static T GetValueWhenSuccessOrThrowInvalidCastException<T, TParam>(TParam param, Func<TParam, OneOf<T, Error<string>>> tryCreateFunc) 
        => tryCreateFunc(param).Match(static arg => arg, static error => throw new InvalidCastException(error.Value));

    public static bool IsEquivalent<T>(this IEnumerable<T> first, IEnumerable<T> second)
    {
        if (ReferenceEquals(first, second))
        {
            return true;
        }
        var firstRes = first.TryGetNonEnumeratedCount(out var firstCount);
        var secondRes = second.TryGetNonEnumeratedCount(out var secondCount);
        if (firstRes && secondRes && firstCount != secondCount)
        {
            return false;
        }
        return first.Order().SequenceEqual(second.Order());
    }
}