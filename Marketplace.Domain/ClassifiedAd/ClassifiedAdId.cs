using System;
using Marketplace.EventSourcing;

namespace Marketplace.Domain.ClassifiedAd;

public class ClassifiedAdId : TypedId
{
    public ClassifiedAdId(Guid value) : base(value)
    {
    }

    public static implicit operator ClassifiedAdId(Guid guid)
    {
        return new(guid);
    }
}