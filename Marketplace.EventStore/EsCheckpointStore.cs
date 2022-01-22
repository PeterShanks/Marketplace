using System.Text;
using EventStore.ClientAPI;
using Marketplace.EventSourcing;
using Newtonsoft.Json;

namespace Marketplace.EventStore;

public class EsCheckpointStore : ICheckpointStore
{
    private const string CheckpointStreamPrefix = "checkpoint:";
    private readonly IEventStoreConnection _connection;
    private readonly string _streamName;

    public EsCheckpointStore(
        IEventStoreConnection connection,
        string subscriptionName)
    {
        _connection = connection;
        _streamName = $"{CheckpointStreamPrefix}{subscriptionName}";
    }

    public async Task<long?> GetCheckpoint()
    {
        var slice = await _connection
            .ReadStreamEventsBackwardAsync(_streamName, -1, 1, false);

        var eventData = slice.Events.FirstOrDefault();

        if (eventData.Equals(default(ResolvedEvent)))
        {
            await StoreCheckpoint(AllCheckpoint.AllStart?.CommitPosition);
            await SetStreamMaxCount();
            return null;
        }

        return eventData.Deserialize<Checkpoint>()?.Position;
    }

    public Task StoreCheckpoint(long? checkpoint)
    {
        var @event = new Checkpoint {Position = checkpoint};
        var preparedEvent =
            new EventData(
                Guid.NewGuid(),
                "$checkpoint",
                true,
                Encoding.UTF8.GetBytes(
                    JsonConvert.SerializeObject(@event)
                ),
                null);

        return _connection.AppendToStreamAsync(
            _streamName,
            ExpectedVersion.Any,
            preparedEvent);
    }

    private async Task SetStreamMaxCount()
    {
        var metadata = await _connection.GetStreamMetadataAsync(_streamName);

        if (!metadata.StreamMetadata.MaxCount.HasValue)
            await _connection.SetStreamMetadataAsync(
                _streamName, ExpectedVersion.Any,
                StreamMetadata.Create(1)
            );
    }

    private class Checkpoint
    {
        public long? Position { get; set; }
    }
}