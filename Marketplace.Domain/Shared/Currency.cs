using System.Collections.Generic;
using Marketplace.EventSourcing;

namespace Marketplace.Domain.Shared;

public class Currency : ValueObject
{
    public static Currency None = new() {InUse = false};
    public string CurrencyCode { get; set; }
    public bool InUse { get; set; }
    public int DecimalPlaces { get; set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return CurrencyCode;
        yield return InUse;
        yield return DecimalPlaces;
    }
}