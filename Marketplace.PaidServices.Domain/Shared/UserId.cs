using Marketplace.EventSourcing;

namespace Marketplace.PaidServices.Domain.Shared;

public class UserId : TypedId
{
    public UserId(Guid value) : base(value)
    {
    }

    public static UserId FromGuid(Guid value)
    {
        return new(value);
    }
}