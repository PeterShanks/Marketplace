using System;

namespace Marketplace.EventSourcing;

public abstract class Entity<TId> : IInternalEventHandler
    where TId : TypedId
{
    private readonly Action<object> _applier;

    protected Entity(Action<object> applier)
    {
        _applier = applier;
    }

    public TId Id { get; protected set; }

    void IInternalEventHandler.Handle(object @event)
    {
        When(@event);
    }

    /// <summary>
    ///     The When method should never fail. instead validating the state should fail on the aggregate root
    /// </summary>
    /// <param name="event"></param>
    protected abstract void When(object @event);

    protected void Apply(object @event)
    {
        When(@event);
        _applier(@event);
    }
}