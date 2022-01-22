using Marketplace.EventSourcing;

namespace Marketplace.Ads.Domain.ClassifiedAds;

public class ClassifiedAdId : AggregateId<ClassifiedAd>
{
    public ClassifiedAdId(Guid value) : base(value)
    {
    }

    public static ClassifiedAdId FromGuid(Guid value)
    {
        return new(value);
    }
}