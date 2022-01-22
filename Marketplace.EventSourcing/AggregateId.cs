using System;

namespace Marketplace.EventSourcing;

public class AggregateId<T> : TypedId
    where T : AggregateRoot
{
    public AggregateId(Guid value) : base(value)
    {
    }
}