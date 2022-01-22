using Marketplace.EventSourcing;
using Marketplace.PaidServices.Domain.Services;
using static Marketplace.PaidServices.Messages.Orders.Events;


namespace Marketplace.PaidServices.Domain.Orders;

internal enum OrderStatus
{
    New,
    Completed
}

public class OrderState : AggregateState<OrderState>
{
    internal Guid AdId { get; set; }
    private OrderStatus Status { get; set; } = OrderStatus.New;
    internal List<PaidService> Services { get; } = new();

    public override OrderState When(OrderState state, object @event)
    {
        return With(
            @event switch
            {
                V1.OrderCreated e =>
                    With(state, x => x.AdId = e.ClassifiedAdId),
                V1.ServiceAddedToOrder e =>
                    With(state, x => x.Services.Add(PaidService.Find(e.ServiceType))),
                V1.ServiceRemovedFromOrder e =>
                    With(
                        state, x => Services.RemoveAll(
                            s => s.Type == PaidService.ParseString(e.ServiceType)
                        )
                    ),
                V1.OrderFulfilled _ =>
                    With(state, x => x.Status = OrderStatus.Completed),
                _ => this
            },
            x => x.Version++
        );
    }

    protected override bool EnsureValidState(OrderState newState)
    {
        return newState switch
        {
            { } o when o.AdId == Guid.Empty => false,
            { } o when o.Status == OrderStatus.Completed
                       && o.Services.Count != Services.Count
                => false,
            _ => true
        };
    }

    internal string[] GetServices()
    {
        return Services
            .Select(x => x.Type.ToString())
            .ToArray();
    }

    internal double GetTotal()
    {
        return Services.Sum(x => x.Price);
    }
}