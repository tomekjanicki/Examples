using MessagePack;

namespace Code.Models;

[MessagePackObject]
public sealed class ClassWithConstructorAndReadOnlyCollectionProperty
{
    [SerializationConstructor]
    public ClassWithConstructorAndReadOnlyCollectionProperty(IReadOnlyCollection<int> items)
    {
        Items = items;
    }

    [Key(0)]
    public IReadOnlyCollection<int> Items { get; }
}