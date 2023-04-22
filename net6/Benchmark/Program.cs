using BenchmarkDotNet.Running;

var benchmark = new Benchmark6.Benchmark
{
    N = 10
};
benchmark.Setup();

var jSerialize = benchmark.JsonSerialize();
var mpIntSerialize = benchmark.MessagePackIntKeySerialize();
var mpStringSerialize = benchmark.MessagePackStringKeySerialize();

Console.WriteLine($"jSerialize len: {jSerialize.Length}");
Console.WriteLine($"mpIntSerialize len: {mpIntSerialize.Length}");
Console.WriteLine($"mpStringSerialize len: {mpStringSerialize.Length}");

var jDeserialize = benchmark.JsonDeserialize();
var mIntDeserialize = benchmark.MessagePackIntKeyDeserialize();
var mStringDeserialize = benchmark.MessagePackStringKeyDeserialize();

Console.WriteLine(jDeserialize.Count);
Console.WriteLine(mIntDeserialize.Count);
Console.WriteLine(mStringDeserialize.Count);

BenchmarkRunner.Run<Benchmark6.Benchmark>();