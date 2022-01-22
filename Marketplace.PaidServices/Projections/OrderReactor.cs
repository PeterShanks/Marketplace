using Marketplace.EventStore;
using Marketplace.PaidServices.ClassifiedAds;
using Marketplace.PaidServices.Messages.Orders;
using Microsoft.Extensions.Logging;
using Commands = Marketplace.PaidServices.Messages.Ads.Commands;

namespace Marketplace.PaidServices.Projections;

public class OrderReactor : EventStoreReactor
{
    public OrderReactor(
        ClassifiedAdCommandService service,
        ILogger<OrderReactor> logger
    ) : base(
        @event => React(service, @event),
        logger
    )
    {
    }

    private static Func<Task> React(
        ClassifiedAdCommandService service,
        object @event
    )
    {
        return @event switch
        {
            Events.V1.OrderCreated e =>
                () => service.Handle(
                    new Commands.V1.Create
                    {
                        ClassifiedAdId = e.ClassifiedAdId,
                        SellerId = e.CustomerId
                    }
                ),
            Events.V1.OrderFulfilled e =>
                () => service.Handle(
                    new Commands.V1.FulFillOrder
                    {
                        ClassifiedAdId = e.ClassifiedAdId,
                        When = e.FulfilledAt,
                        ServiceTypes = e.Services
                            .Select(x => x.Type)
                            .ToArray()
                    }
                ),
            _ => () => Task.CompletedTask
        };
    }
}