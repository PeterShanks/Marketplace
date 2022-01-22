using Marketplace.EventSourcing;

namespace Marketplace.Ads.Domain.Shared;

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