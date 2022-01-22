using Marketplace.EventSourcing;

namespace Marketplace.PaidServices.Domain.Orders;

public class OrderId : TypedId
{
    public OrderId(Guid value) : base(value)
    {
    }

    public static OrderId FromGuid(Guid value)
    {
        return new(value);
    }
}