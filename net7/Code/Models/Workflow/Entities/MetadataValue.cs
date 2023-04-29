namespace Code.Models.Workflow.Entities;

public readonly record struct MetadataValue
{
    private MetadataValue(int? intValue, string? stringValue, Guid? guidValue, DateTime? dateTimeValue, short? shortValue, bool? boolValue,
        long? longValue, byte? byteValue)
    {
        IntValue = intValue;
        StringValue = stringValue;
        GuidValue = guidValue;
        DateTimeValue = dateTimeValue;
        ShortValue = shortValue;
        BoolValue = boolValue;
        LongValue = longValue;
        ByteValue = byteValue;
    }

    public MetadataValue(int? intValue)
        : this(intValue, null, null, null, null, null, null, null)
    {
    }

    public MetadataValue(short? shortValue)
        : this(null, null, null, null, shortValue, null, null, null)
    {
    }

    public MetadataValue(Guid? guidValue)
        : this(null, null, guidValue, null, null, null, null, null)
    {
    }

    public MetadataValue(string? stringValue)
        : this(null, stringValue, null, null, null, null, null, null)
    {
    }

    public MetadataValue(DateTime? dateTimeValue)
        : this(null, null, null, dateTimeValue, null, null, null, null)
    {
    }

    public MetadataValue(bool? boolValue)
        : this(null, null, null, null, null, boolValue, null, null)
    {
    }

    public MetadataValue(long? longValue)
        : this(null, null, null, null, null, null, longValue, null)
    {
    }

    public MetadataValue(byte? byteValue)
        : this(null, null, null, null, null, null, null, byteValue)
    {
    }

    public static implicit operator MetadataValue(string? value) => new(value);

    public static implicit operator MetadataValue(DateTime? value) => new(value);

    public static implicit operator MetadataValue(bool? value) => new(value);

    public static implicit operator MetadataValue(DateTime value) => new(value);

    public static implicit operator MetadataValue(Guid? value) => new(value);

    public static implicit operator MetadataValue(Guid value) => new(value);

    public long? LongValue { get; }

    public int? IntValue { get; }

    public string? StringValue { get; }

    public Guid? GuidValue { get; }

    public DateTime? DateTimeValue { get; }

    public short? ShortValue { get; }

    public bool? BoolValue { get; }

    public byte? ByteValue { get; }

    public bool IsNull()
        => IntValue is null && StringValue is null && GuidValue is null && DateTimeValue is null && ShortValue is null && BoolValue is null && LongValue is null && ByteValue is null;
}