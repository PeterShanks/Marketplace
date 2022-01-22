﻿using System.Collections.Generic;
using System.Linq;
using Marketplace.Ads.Domain.Shared;

namespace Marketplace.Infrastructure.Currency;

public class FixedCurrencyLookup : ICurrencyLookup
{
    private static readonly IEnumerable<Ads.Domain.Shared.Currency> _currencies =
        new[]
        {
            new Ads.Domain.Shared.Currency
            {
                CurrencyCode = "EUR",
                DecimalPlaces = 2,
                InUse = true
            },
            new Ads.Domain.Shared.Currency
            {
                CurrencyCode = "USD",
                DecimalPlaces = 2,
                InUse = true
            }
        };

    public Ads.Domain.Shared.Currency FindCurrency(string currencyCode)
    {
        var currency = _currencies.FirstOrDefault(x => x.CurrencyCode == currencyCode);
        return currency ?? Ads.Domain.Shared.Currency.None;
    }
}