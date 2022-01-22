using System;
using System.Collections.Generic;
using System.Linq;
using Force.DeepCloner;

namespace Marketplace.EventSourcing;

public interface IAggregateState<T>
{
    string StreamName { get; }
    long Version { get; }
    T When(T state, object @event);
    string GetStreamName(Guid id);
}

public abstract class AggregateState<T> : IAggregateState<T>
    where T : class, new()
{
    public Guid Id { get; protected set; }
    public abstract T When(T state, object @event);

    public string GetStreamName(Guid id)
    {
        return $"{typeof(T).Name}-{id:N}";
    }

    public string StreamName => GetStreamName(Id);
    public long Version { get; protected set; }

    protected T With(T state, Action<T> update)
    {
        update(state);
        return state;
    }

    protected abstract bool EnsureValidState(T newState);

    private T Apply(T state, object @event)
    {
        var newState = state.DeepClone();
        newState = When(state, @event);

        if (!EnsureValidState(newState))
            throw new InvalidEntityStateException(
                this,
                "Post-checks failed"
            );

        return newState;
    }

    public Result NoEvents()
    {
        return new Result(this as T, new List<object>());
    }

    public Result Apply(params object[] events)
    {
        var newState = this as T;
        newState = events.Aggregate(newState, Apply);
        return new Result(newState, events);
    }

    public class Result
    {
        public Result(T state, IEnumerable<object> events)
        {
            State = state;
            Events = events;
        }

        public T State { get; }
        public IEnumerable<object> Events { get; }
    }
}