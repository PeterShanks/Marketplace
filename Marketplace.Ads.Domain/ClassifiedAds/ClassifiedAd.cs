using Marketplace.Ads.Domain.Shared;
using Marketplace.EventSourcing;
using static Marketplace.Ads.Messages.Events;

namespace Marketplace.Ads.Domain.ClassifiedAds;

public class ClassifiedAd : AggregateRoot
{
    private List<Picture> _pictures;

    // Aggregate state properties
    public UserId OwnerId { get; private set; }
    public ClassifiedAdTitle Title { get; private set; }
    public ClassifiedAdText Text { get; private set; }
    public Price Price { get; private set; }
    public ClassifiedAdState State { get; set; }
    public UserId ApprovedBy { get; private set; }
    public IReadOnlyList<Picture> Pictures => _pictures.AsReadOnly();

    public static ClassifiedAd Create(ClassifiedAdId id, UserId ownerId)
    {
        var ad = new ClassifiedAd();

        ad.Apply(new V1.ClassifiedAdCreated
        {
            Id = id,
            OwnerId = ownerId
        });

        return ad;
    }

    public void SetTitle(ClassifiedAdTitle title)
    {
        Apply(new V1.ClassifiedAdTitleChanged
        {
            Id = Id,
            OwnerId = OwnerId,
            Title = title
        });
    }

    public void UpdateText(ClassifiedAdText text)
    {
        Apply(new V1.ClassifiedAdTextUpdated
        {
            Id = Id,
            OwnerId = OwnerId,
            AdText = text
        });
    }

    public void UpdatePrice(Price price)
    {
        Apply(new V1.ClassifiedAdPriceUpdated
        {
            Id = Id,
            OwnerId = OwnerId,
            Price = price.Amount,
            CurrencyCode = price.Currency.CurrencyCode
        });
    }

    public void RequestToPublish()
    {
        Apply(new V1.ClassifiedAdSentForReview
        {
            Id = Id,
            OwnerId = OwnerId
        });
    }

    public void AddPicture(string pictureUri, PictureSize size)
    {
        Apply(new V1.PictureAddedToAClassifiedAd
        {
            PictureId = new Guid(),
            ClassifiedAdId = Id,
            OwnerId = OwnerId,
            Url = pictureUri,
            Height = size.Height,
            Width = size.Width,
            Order = Pictures.Any() ? Pictures.Max(x => x.Order) : 0
        });
    }

    public void Publish(UserId userId)
    {
        Apply(new V1.ClassifiedAdPublished
        {
            Id = Id,
            ApprovedBy = userId,
            OwnerId = OwnerId
        });
    }

    public void Delete()
    {
        Apply(
            new V1.ClassifiedAdDeleted
            {
                Id = Id,
                OwnerId = OwnerId
            }
        );
    }

    /// <exception cref="InvalidOperationException">Cannot resize a picture that I don't have</exception>
    public void ResizePicture(PictureId pictureId, PictureSize newSize)
    {
        var picture = FindPicture(pictureId);
        if (picture == null)
            throw new InvalidOperationException("Cannot resize a picture that I don't have");

        picture.Resize(newSize);
    }

    protected override void When(object @event)
    {
        Picture picture;

        switch (@event)
        {
            case V1.ClassifiedAdCreated e:
                Id = new ClassifiedAdId(e.Id);
                OwnerId = new UserId(e.OwnerId);
                State = ClassifiedAdState.Inactive;
                _pictures = new List<Picture>();
                break;

            case V1.ClassifiedAdTitleChanged e:
                Title = new ClassifiedAdTitle(e.Title);
                break;

            case V1.ClassifiedAdTextUpdated e:
                Text = new ClassifiedAdText(e.AdText);
                break;

            case V1.ClassifiedAdPriceUpdated e:
                Price = new Price(e.Price, e.CurrencyCode);
                break;

            case V1.ClassifiedAdSentForReview:
                State = ClassifiedAdState.PendingReview;
                break;

            case V1.ClassifiedAdPublished e:
                ApprovedBy = new UserId(e.ApprovedBy);
                State = ClassifiedAdState.Active;
                break;

            // picture
            case V1.PictureAddedToAClassifiedAd e:
                picture = new Picture(Apply);
                ApplyToEntity(picture, e);
                _pictures.Add(picture);
                break;

            case V1.ClassifiedAdPictureResized e:
                picture = FindPicture(new PictureId(e.PictureId));
                ApplyToEntity(picture, e);
                break;
        }
    }

    private Picture FindPicture(PictureId id)
    {
        return Pictures.FirstOrDefault(x => x.Id == id);
    }

    /// <exception cref="DomainExceptions.InvalidEntityStateException">Entity steps into invalid state.</exception>
    protected override void EnsureValidState()
    {
        var valid =
            OwnerId != null &&
            State switch
            {
                ClassifiedAdState.PendingReview =>
                    Title != null
                    && Text != null
                    && Price?.Amount > 0,
                ClassifiedAdState.Active =>
                    Title != null &&
                    Text != null &&
                    Price?.Amount > 0 &&
                    ApprovedBy != null,
                _ => true
            };

        if (!valid)
            throw new DomainExceptions.InvalidEntityStateException(this, $"Post-checks failed in state {State}");
    }
}