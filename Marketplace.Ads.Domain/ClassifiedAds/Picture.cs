using Marketplace.EventSourcing;
using static Marketplace.Ads.Messages.Events;


namespace Marketplace.Ads.Domain.ClassifiedAds;

public class Picture : Entity<PictureId>
{
    public Picture(Action<object> applier) : base(applier)
    {
    }

    internal ClassifiedAdId ParentId { get; private set; }
    internal PictureSize Size { get; private set; }
    internal Uri Location { get; private set; }
    internal int Order { get; private set; }


    protected override void When(object @event)
    {
        switch (@event)
        {
            case V1.PictureAddedToAClassifiedAd e:
                ParentId = new ClassifiedAdId(e.ClassifiedAdId);
                Id = new PictureId(e.PictureId);
                Location = new Uri(e.Url);
                Size = new PictureSize {Height = e.Height, Width = e.Width};
                Order = e.Order;
                break;
            case V1.ClassifiedAdPictureResized e:
                Size = new PictureSize {Height = e.Height, Width = e.Width};
                break;
        }
    }

    public void Resize(PictureSize newSize)
    {
        Apply(new V1.ClassifiedAdPictureResized
        {
            PictureId = Id.Value,
            ClassifiedAdId = ParentId.Value,
            Height = newSize.Width,
            Width = newSize.Width
        });
    }
}