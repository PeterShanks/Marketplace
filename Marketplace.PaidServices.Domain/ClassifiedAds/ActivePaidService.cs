using Marketplace.EventSourcing;
using Marketplace.PaidServices.Domain.Services;

namespace Marketplace.PaidServices.Domain.ClassifiedAds;

public class ActivePaidService : ValueObject
{
    private ActivePaidService(PaidService paidService, DateTimeOffset expiresAt)
    {
        PaidService = paidService;
        ExpiresAt = expiresAt;
    }

    public PaidService PaidService { get; }
    public DateTimeOffset ExpiresAt { get; }

    public static ActivePaidService Create(
        PaidService paidService,
        DateTimeOffset startFrom
    )
    {
        var expiresAt = startFrom + paidService.Duration;
        return new ActivePaidService(paidService, expiresAt);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return PaidService;
        yield return ExpiresAt;
    }
}