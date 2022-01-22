using static Marketplace.Ads.Messages.Events;
using static Marketplace.EventSourcing.TypeMapper;

namespace Marketplace.Ads;

public static class EventMappings
{
    public static void MapEventTypes()
    {
        Map<V1.ClassifiedAdCreated>(nameof(V1.ClassifiedAdCreated));
        Map<V1.ClassifiedAdDeleted>(nameof(V1.ClassifiedAdDeleted));
        Map<V1.ClassifiedAdPublished>(nameof(V1.ClassifiedAdPublished));
        Map<V1.ClassifiedAdTextUpdated>(nameof(V1.ClassifiedAdTextUpdated));
        Map<V1.ClassifiedAdPriceUpdated>(nameof(V1.ClassifiedAdPriceUpdated));
        Map<V1.ClassifiedAdTitleChanged>(nameof(V1.ClassifiedAdTitleChanged));
        Map<V1.ClassifiedAdPictureResized>(nameof(V1.ClassifiedAdPictureResized));
        Map<V1.ClassifiedAdSentForReview>(nameof(V1.ClassifiedAdSentForReview));
        Map<V1.PictureAddedToAClassifiedAd>(nameof(V1.PictureAddedToAClassifiedAd));
    }
}