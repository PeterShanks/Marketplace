using Marketplace.EventSourcing;
using Marketplace.PaidServices.Domain.Orders;
using Marketplace.PaidServices.Messages.Ads;

namespace Marketplace.PaidServices.Domain.ClassifiedAds;

public class ClassifiedAdState : AggregateState<ClassifiedAdState>
{
    private List<ActivePaidService> ActiveServices { get; set; } = new();

    private List<OrderId> OrderIds { get; set; } = new();

    public override ClassifiedAdState When(ClassifiedAdState state, object @event)
    {
        return With(
            @event switch
            {
                Events.V1.Created e =>
                    With(state, x => x.Id = e.ClassifiedAdId)
            },
            x => x.Version++
        );
    }


    protected override bool EnsureValidState(ClassifiedAdState newState)
    {
        return true;
    }
}