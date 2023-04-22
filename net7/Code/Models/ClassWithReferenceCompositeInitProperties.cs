using Code.StronglyTypeIds;

namespace Code.Models;

public sealed class ClassWithReferenceCompositeInitProperties
{
    public required EMail EMail { get; init; }

    public EMail? OptionalEmail { get; init; }
}