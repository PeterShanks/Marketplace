using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Marketplace.EventSourcing;

public abstract class ApplicationService<T> : IApplicationService
    where T : AggregateRoot
{
    private readonly Dictionary<Type, Func<object, Task>> _handlers = new();

    private readonly ILogger _logger;
    private readonly IAggregateStore _store;

    protected ApplicationService(IAggregateStore store, ILogger logger)
    {
        _store = store;
        _logger = logger;
    }

    public Task Handle<TCommand>(TCommand command)
        where TCommand : class
    {
        if (!_handlers.TryGetValue(typeof(TCommand), out var handler))
            throw new InvalidOperationException(
                $"No registered handler for command {typeof(TCommand).Name}"
            );

        _logger.LogDebug("Handling command: {Command}", command.ToString());
        return handler(command);
    }

    protected void CreateWhen<TCommand>(
        Func<TCommand, AggregateId<T>> getAggregateId,
        Func<TCommand, AggregateId<T>, T> creator)
        where TCommand : class
    {
        When<TCommand>(async command =>
            {
                var aggregateId = getAggregateId(command);

                if (await _store.Exists(aggregateId))
                    throw new InvalidOperationException(
                        $"Entity with id {aggregateId} already exists"
                    );

                var aggregate = creator(command, aggregateId);

                await _store.Save(aggregate);
            }
        );
    }

    protected void UpdateWhen<TCommand>(
        Func<TCommand, AggregateId<T>> getAggregateId,
        Action<T, TCommand> updater)
        where TCommand : class
    {
        When<TCommand>(async command =>
            {
                var aggregateId = getAggregateId(command);
                var aggregate = await _store.Load(aggregateId);

                if (aggregate is null)
                    throw new InvalidOperationException(
                        $"Entity with id {aggregateId} does not exist"
                    );

                updater(aggregate, command);
                await _store.Save(aggregate);
            }
        );
    }

    private void When<TCommand>(Func<TCommand, Task> handler)
        where TCommand : class
    {
        _handlers.Add(
            typeof(TCommand),
            c => handler(c as TCommand)
        );
    }
}