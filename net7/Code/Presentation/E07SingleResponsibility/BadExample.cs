using System.Text.Json;
using static Code.Presentation.E07SingleResponsibility.GoodExample;

namespace Code.Presentation.E07SingleResponsibility;

public class BadExample
{
    public interface IMessage
    {
        string Schema { get; }
    }

    public class EventHubAdditionalData
    {
        public EventHubAdditionalData(string traceId)
        {
            TraceId = traceId;
        }

        public string TraceId { get; }
    }

    public sealed class EventHubSender
    {
        public async Task<IEnumerable<KeyValuePair<T, EventHubAdditionalData>>> SendData<T>(IEnumerable<KeyValuePair<T, EventHubAdditionalData>> eventHubData, string eventHubConnectionString)
            where T : IMessage
        {
            var data = eventHubData.Select(pair => new Data<KeyValuePair<T, EventHubAdditionalData>>(pair, pair.Key.Schema));
            var writeBatchResult = await WriteBatch(
                data,
                eventHubConnectionString,
                new Config<KeyValuePair<T, EventHubAdditionalData>>(pair => JsonSerializer.SerializeToUtf8Bytes(pair.Key)));

            return writeBatchResult.Select(static d => d.Item);

        }

        private async Task<IReadOnlyCollection<Data<T>>> WriteBatch<T>(IEnumerable<Data<T>> items, string eventHubConnectionString, Config<T> config)
        {
            var grouped = items.GroupBy(data => data.PartitionKey);
            var results = new List<Data<T>>();
            foreach (var group in grouped)
            {
                results.AddRange(await SendBatch(group.Key, group.Select(data => data.Item), eventHubConnectionString, config));
            }
            return results;
        }

        private Task<IEnumerable<Data<T>>> SendBatch<T>(string? partitionKey, IEnumerable<T> items, string eventHubConnectionString, Config<T> config)
        {
            throw new NotImplementedException();
        }
    }

    public record Data<T>(T Item, string Schema, string? PartitionKey = null);
}