using MessagePack;

namespace Benchmark7;

[MessagePackObject]
public sealed class MessagePackIntKeyTestItem
{
    [SerializationConstructor]
    public MessagePackIntKeyTestItem(int age, string firstName, string lastName)
    {
        Age = age;
        FirstName = firstName;
        LastName = lastName;
    }

    [Key(0)]
    public int Age { get; }

    [Key(1)]
    public string FirstName { get; }

    [Key(2)]
    public string LastName { get; }
}