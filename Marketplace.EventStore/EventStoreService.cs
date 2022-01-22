using EventStore.ClientAPI;
using Microsoft.Extensions.Hosting;

namespace Marketplace.EventStore;

public class EventStoreService : IHostedService
{
    private readonly IEventStoreConnection _connection;
    private readonly IEnumerable<SubscriptionManager> _subscriptions;

    public EventStoreService(
        IEventStoreConnection connection,
        IEnumerable<SubscriptionManager> subscriptionManagers)
    {
        _connection = connection;
        _subscriptions = subscriptionManagers;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _connection.ConnectAsync();
        await Task.WhenAll(
            _subscriptions.Select(s => s.Start())
        );
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        foreach (var subscriptionManager in _subscriptions)
            subscriptionManager.Stop();

        _connection.Close();
        return Task.CompletedTask;
    }
}