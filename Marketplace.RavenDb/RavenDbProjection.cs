using Marketplace.EventSourcing;
using Microsoft.Extensions.Logging;
using Raven.Client.Documents.Session;

namespace Marketplace.RavenDb;

public class RavenDbProjection<T> : ISubscription
{
    public delegate Func<Task> Projector(
        IAsyncDocumentSession session,
        object @event
    );

    private readonly GetSession _getSession;
    private readonly ILogger _logger;
    private readonly Projector _projector;
    private readonly string ReadModelName = typeof(T).Name;

    public RavenDbProjection(
        ILogger<RavenDbProjection<T>> logger,
        GetSession session,
        Projector projector)
    {
        _logger = logger;
        _getSession = session;
        _projector = projector;
    }

    public async Task Project(object @event)
    {
        using var session = _getSession();

        var handler = _projector(session, @event);

        if (handler == null) return;

        _logger.LogDebug("Projecting {Event} to {Model}", @event, ReadModelName);

        await handler();
        await session.SaveChangesAsync();
    }
}