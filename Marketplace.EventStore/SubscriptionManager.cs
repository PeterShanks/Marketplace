using EventStore.ClientAPI;
using Marketplace.EventSourcing;
using Microsoft.Extensions.Logging;

namespace Marketplace.EventStore;

public class SubscriptionManager
{
    private readonly ICheckpointStore _checkpoint;
    private readonly IEventStoreConnection _connection;
    private readonly ILogger<SubscriptionManager> _logger;
    private readonly string _name;
    private readonly ISubscription[] _subscriptions;
    private EventStoreAllCatchUpSubscription _subscription;

    public SubscriptionManager(
        IEventStoreConnection connection,
        ICheckpointStore checkpointStore,
        string name,
        ILogger<SubscriptionManager> logger,
        params ISubscription[] subscriptions)
    {
        _connection = connection;
        _checkpoint = checkpointStore;
        _name = name;
        _subscriptions = subscriptions;
        _logger = logger;
    }

    public async Task Start()
    {
        var settings = new CatchUpSubscriptionSettings(
            2000, 500,
            _logger.IsEnabled(LogLevel.Trace),
            false, _name);

        _logger.LogDebug("Starting the projection manager...");
        var position = await _checkpoint.GetCheckpoint();
        _logger.LogDebug("Retrieved the checkpoint: {Checkpoint}", position);

        _subscription = _connection.SubscribeToAllFrom(
            GetPosition(),
            settings,
            EventAppeared
        );

        _logger.LogDebug("Subscribed to $all stream");

        Position? GetPosition()
        {
            return position.HasValue
                ? new Position(position.Value, position.Value)
                : AllCheckpoint.AllStart;
        }
    }

    public void Stop()
    {
        _subscription.Stop();
    }

    private async Task EventAppeared(
        EventStoreCatchUpSubscription _,
        ResolvedEvent resolvedEvent)
    {
        if (resolvedEvent.Event.EventType.StartsWith("$"))
            return;

        var @event = resolvedEvent.Deserialize();

        _logger.LogDebug("Projecting event {event}", @event.ToString());

        try
        {
            await Task.WhenAll(_subscriptions.Select(x => x.Project(@event)));

            await _checkpoint.StoreCheckpoint(
                resolvedEvent.OriginalPosition.Value.CommitPosition);
        }
        catch (Exception e)
        {
            _logger.LogError(
                e,
                "Error occurred when projecting the event {event}",
                @event
            );
            throw;
        }
    }
}