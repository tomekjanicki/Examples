using Code.StronglyTypeIds;
using MessagePack;

namespace Code.Models;

[MessagePackObject]
public sealed class ClassWithConstructorAndReferenceCompositeProperties
{
    [SerializationConstructor]
    public ClassWithConstructorAndReferenceCompositeProperties(EMail eMail, EMail? optionalEmail)
    {
        EMail = eMail;
        OptionalEmail = optionalEmail;
    }

    [Key(0)]
    public EMail EMail { get; }

    [Key(1)]
    public EMail? OptionalEmail { get; }
}