namespace Benchmark6;

public sealed class JsonTestItem
{
    public JsonTestItem(int age, string firstName, string lastName)
    {
        Age = age;
        FirstName = firstName;
        LastName = lastName;
    }

    public int Age { get; }

    public string FirstName { get; }

    public string LastName { get; }
}