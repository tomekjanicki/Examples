using OneOf;
using OneOf.Types;

namespace Code;

public static class Extensions
{
    public static T GetValueWhenSuccessOrThrowInvalidCastException<T, TParam>(TParam param, Func<TParam, OneOf<T, Error<string>>> tryCreateFunc) 
        => tryCreateFunc(param).Match(static arg => arg, static error => throw new InvalidCastException(error.Value));
}