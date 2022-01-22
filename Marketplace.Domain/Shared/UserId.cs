using System;
using Marketplace.EventSourcing;

namespace Marketplace.Domain.Shared;

public class UserId : TypedId
{
    public UserId(Guid value) : base(value)
    {
    }

    public static implicit operator UserId(Guid value) => new UserId(value);
}