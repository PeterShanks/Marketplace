namespace Marketplace.Ads.Domain.Shared;

public interface ICurrencyLookup
{
    Currency FindCurrency(string currencyCode);
}