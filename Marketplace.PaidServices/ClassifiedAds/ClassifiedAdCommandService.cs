using Marketplace.EventSourcing;
using Marketplace.PaidServices.Domain.ClassifiedAds;
using Marketplace.PaidServices.Domain.Services;
using Marketplace.PaidServices.Domain.Shared;
using static Marketplace.PaidServices.Messages.Ads.Commands;


namespace Marketplace.PaidServices.ClassifiedAds;

public class ClassifiedAdCommandService : CommandService<ClassifiedAdState>
{
    public ClassifiedAdCommandService(IFunctionalAggregateStore store) : base(store)
    {
    }

    public Task Handle(V1.Create command)
    {
        return Handle(
            command.ClassifiedAdId,
            state => ClassifiedAd.Create(
                ClassifiedAdId.FromGuid(command.ClassifiedAdId),
                UserId.FromGuid(command.SellerId)
            )
        );
    }

    public Task Handle(V1.FulFillOrder command)
    {
        return Handle(
            command.ClassifiedAdId,
            state => ClassifiedAd.FulfillOrder(
                state,
                command.When,
                command.ServiceTypes.Select(PaidService.Find)
            )
        );
    }
}