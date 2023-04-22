using System.Text.Json;
using BenchmarkDotNet.Attributes;
using MessagePack;

namespace Benchmark6;

[MemoryDiagnoser]
public class Benchmark
{
    private IReadOnlyCollection<MessagePackIntKeyTestItem> _mpIntItems = null!;
    private IReadOnlyCollection<MessagePackStringKeyTestItem> _mpStringItems = null!;
    private IReadOnlyCollection<JsonTestItem> _jItems = null!;
    private byte[] _mpIntData = null!;
    private byte[] _mpStringData = null!;
    private byte[] _jData = null!;

    [Params(10, 100)]
    public int N;

    [GlobalSetup]
    public void Setup()
    {
        _mpIntItems = Enumerable.Range(0, N)
            .Select(i => new MessagePackIntKeyTestItem(i, $"firstName{i}", $"lastName{i}")).ToArray();
        _mpStringItems = Enumerable.Range(0, N)
            .Select(i => new MessagePackStringKeyTestItem(i, $"firstName{i}", $"lastName{i}")).ToArray();
        _jItems = Enumerable.Range(0, N)
            .Select(i => new JsonTestItem(i, $"firstName{i}", $"lastName{i}")).ToArray();
        _jData = JsonSerializer.SerializeToUtf8Bytes(_jItems);
        _mpIntData = MessagePackSerializer.Serialize(_mpIntItems);
        _mpStringData = MessagePackSerializer.Serialize(_mpStringItems);
    }

    [Benchmark]
    public byte[] JsonSerialize()
    {
        return JsonSerializer.SerializeToUtf8Bytes(_jItems);
    }

    [Benchmark]
    public byte[] MessagePackIntKeySerialize()
    {
        return MessagePackSerializer.Serialize(_mpIntItems);
    }

    [Benchmark]
    public byte[] MessagePackStringKeySerialize()
    {
        return MessagePackSerializer.Serialize(_mpStringItems);
    }

    [Benchmark]
    public IReadOnlyCollection<JsonTestItem> JsonDeserialize()
    {
        return JsonSerializer.Deserialize<IReadOnlyCollection<JsonTestItem>>(_jData)!;
    }

    [Benchmark]
    public IReadOnlyCollection<MessagePackIntKeyTestItem> MessagePackIntKeyDeserialize()
    {
        return MessagePackSerializer.Deserialize<IReadOnlyCollection<MessagePackIntKeyTestItem>>(_mpIntData);
    }

    [Benchmark]
    public IReadOnlyCollection<MessagePackStringKeyTestItem> MessagePackStringKeyDeserialize()
    {
        return MessagePackSerializer.Deserialize<IReadOnlyCollection<MessagePackStringKeyTestItem>>(_mpStringData);
    }
}