using Marketplace.EventSourcing;

namespace Marketplace.Ads.Domain.ClassifiedAds;

public class PictureId : TypedId
{
    public PictureId(Guid value) : base(value)
    {
    }
}