namespace Code.Models;

public sealed class ClassWithReadOnlyCollectionInitProperty
{
    public required IReadOnlyCollection<int> Items { get; init; }
}