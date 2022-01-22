using Marketplace.EventSourcing;
using Microsoft.Extensions.Logging;

namespace Marketplace.EventStore;

public abstract class EventStoreReactor : ISubscription
{
    public delegate Func<Task> Reactor(object @event);

    private readonly ILogger<EventStoreReactor> _logger;
    private readonly Reactor _reactor;

    public EventStoreReactor(
        Reactor reactor,
        ILogger<EventStoreReactor> logger)
    {
        _reactor = reactor;
        _logger = logger;
    }

    public Task Project(object @event)
    {
        var handler = _reactor(@event);

        if (handler == null) return Task.CompletedTask;

        _logger.LogDebug("Reacting to event {event}", @event);

        return handler();
    }
}