using System;
using Marketplace.EventSourcing;

namespace Marketplace.Domain.ClassifiedAd;

public class PictureId : TypedId
{
    public PictureId(Guid value) : base(value)
    {
    }
}