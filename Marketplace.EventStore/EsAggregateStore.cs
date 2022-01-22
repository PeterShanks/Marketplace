using EventStore.ClientAPI;
using Marketplace.EventSourcing;
using Microsoft.Extensions.Logging;

namespace Marketplace.EventStore;

public class EsAggregateStore : IAggregateStore
{
    private readonly IEventStoreConnection _connection;
    private readonly ILogger<EsAggregateStore> _logger;

    public EsAggregateStore(
        IEventStoreConnection connection,
        ILogger<EsAggregateStore> logger)
    {
        _connection = connection;
        _logger = logger;
    }

    public async Task<bool> Exists<T>(AggregateId<T> aggregateId) where T : AggregateRoot
    {
        var streamName = GetStreamName(aggregateId);
        var result = await _connection.ReadEventAsync(streamName, 1, false);
        return result.Status != EventReadStatus.NoStream;
    }

    public async Task<T> Load<T>(AggregateId<T> aggregateId) where T : AggregateRoot
    {
        if (aggregateId == null)
            throw new ArgumentNullException(nameof(aggregateId));

        var streamName = GetStreamName(aggregateId);
        var aggregate = Activator.CreateInstance(typeof(T), true) as T;

        var page = await _connection.ReadStreamEventsForwardAsync(
            streamName, 0, 1024, false
        );

        _logger.LogDebug("Loading events for the aggregate {aggregate}", aggregate.ToString());

        aggregate.Load(
            page.Events.Select(
                resolvedEvent => resolvedEvent.Deserialize()
            ).ToArray()
        );

        return aggregate;
    }

    public async Task Save<T>(T aggregate) where T : AggregateRoot
    {
        if (aggregate is null)
            throw new ArgumentNullException(nameof(aggregate));

        var streamName = GetStreamName(aggregate);
        var changes = aggregate.GetChanges().ToArray();

        foreach (var change in changes)
            _logger.LogDebug("Persisting event {event}", change.ToString());

        await _connection.AppendEvents(
            streamName,
            aggregate.Version,
            changes);

        aggregate.ClearChanges();
    }

    private static string GetStreamName<T>(AggregateId<T> aggregateId)
        where T : AggregateRoot
    {
        return $"{typeof(T).Name}-{aggregateId}";
    }

    private static string GetStreamName<T>(T aggregate)
        where T : AggregateRoot
    {
        return $"{typeof(T).Name}-{aggregate.Id}";
    }
}