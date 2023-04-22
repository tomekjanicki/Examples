using MessagePack;

namespace Benchmark6;

[MessagePackObject]
public sealed class MessagePackStringKeyTestItem
{
    [SerializationConstructor]
    public MessagePackStringKeyTestItem(int age, string firstName, string lastName)
    {
        Age = age;
        FirstName = firstName;
        LastName = lastName;
    }

    [Key("age")]
    public int Age { get; }

    [Key("firstName")]
    public string FirstName { get; }

    [Key("lastName")]
    public string LastName { get; }
}