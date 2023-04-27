using Code.StronglyTypeIds;
using MessagePack;

namespace Code.Models;

[MessagePackObject]
public sealed class ClassWithReferenceCompositeInitProperties
{
    [Key(0)]
    public required EMail EMail { get; init; }

    [Key(1)]
    public EMail? OptionalEmail { get; init; }
}