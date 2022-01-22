using Marketplace.EventSourcing;
using Marketplace.PaidServices.Domain.Services;
using Marketplace.PaidServices.Domain.Shared;
using static Marketplace.PaidServices.Messages.Ads.Events;


namespace Marketplace.PaidServices.Domain.ClassifiedAds;

public static class ClassifiedAd
{
    public static AggregateState<ClassifiedAdState>.Result Create(
        ClassifiedAdId id,
        UserId sellerId
    )
    {
        return new ClassifiedAdState().Apply(
            new V1.Created
            {
                ClassifiedAdId = id,
                SellerId = sellerId
            }
        );
    }

    public static AggregateState<ClassifiedAdState>.Result FulfillOrder(
        ClassifiedAdState state,
        DateTimeOffset when,
        IEnumerable<PaidService> services
    )
    {
        return state.Apply(
            services.Select(
                x => new V1.ServiceActivated
                {
                    ClassifiedAdId = state.Id,
                    ServiceType = x.Type.ToString(),
                    ActiveUntil = when + x.Duration
                }
            )
        );
    }
}