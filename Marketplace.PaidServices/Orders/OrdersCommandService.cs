using Marketplace.EventSourcing;
using Marketplace.PaidServices.Domain.ClassifiedAds;
using Marketplace.PaidServices.Domain.Orders;
using Marketplace.PaidServices.Domain.Services;
using Marketplace.PaidServices.Domain.Shared;
using static Marketplace.PaidServices.Messages.Orders.Commands;


namespace Marketplace.PaidServices.Orders;

public class OrdersCommandService : CommandService<OrderState>
{
    public OrdersCommandService(IFunctionalAggregateStore store) : base(store)
    {
    }

    public Task Handle(V1.CreateOrder command)
    {
        return Handle(
            command.OrderId,
            state => Order.Create(
                OrderId.FromGuid(command.OrderId),
                ClassifiedAdId.FromGuid(command.ClassifiedAdId),
                UserId.FromGuid(command.CustomerId)
            )
        );
    }

    public Task Handle(V1.AddService command)
    {
        return Handle(
            command.OrderId,
            state => Order.AddService(
                state,
                PaidService.Find(command.ServiceType)
            )
        );
    }

    public Task Handle(V1.RemoveService command)
    {
        return Handle(
            command.OrderId,
            state => Order.RemoveService(
                state, PaidService.Find(command.ServiceType)
            )
        );
    }

    public Task Handle(V1.FulfillOrder command)
    {
        return Handle(command.OrderId, Order.Fulfill);
    }
}