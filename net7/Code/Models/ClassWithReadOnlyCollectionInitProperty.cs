using MessagePack;

namespace Code.Models;

[MessagePackObject]
public sealed class ClassWithReadOnlyCollectionInitProperty
{
    [Key(0)]
    public required IReadOnlyCollection<int> Items { get; init; }
}