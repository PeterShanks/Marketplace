using Marketplace.EventSourcing;

namespace Marketplace.PaidServices.Domain.ClassifiedAds;

public class ClassifiedAdId : TypedId
{
    public ClassifiedAdId(Guid value) : base(value)
    {
    }

    public static ClassifiedAdId FromGuid(Guid value)
    {
        return new(value);
    }
}