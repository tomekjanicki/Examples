namespace Code.Types;

public static class EqualityHelper<T>
    where T : class
{
    public static bool Equals(T? me, T? other, Func<T, T, bool> compareFunc)
    {
        if (me is null || other is null)
        {
            return false;
        }

        return ReferenceEquals(me, other) || compareFunc(me, other);
    }

    public static bool Equals(T me, object? obj, Func<T, T?, bool> func) => ReferenceEquals(me, obj) || obj is T other && func(me, other);
}