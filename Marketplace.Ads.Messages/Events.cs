namespace Marketplace.Ads.Messages;

public static class Events
{
    public static class V1
    {
        public class ClassifiedAdCreated
        {
            public Guid Id { get; set; }
            public Guid OwnerId { get; set; }

            public override string ToString()
            {
                return $"{nameof(ClassifiedAdCreated)}";
            }
        }

        public class ClassifiedAdTitleChanged
        {
            public Guid Id { get; set; }
            public Guid OwnerId { get; set; }
            public string Title { get; set; }

            public override string ToString()
            {
                return $"{nameof(ClassifiedAdTitleChanged)}";
            }
        }

        public class ClassifiedAdTextUpdated
        {
            public Guid Id { get; set; }
            public Guid OwnerId { get; set; }
            public string AdText { get; set; }

            public override string ToString()
            {
                return $"{nameof(ClassifiedAdTextUpdated)}";
            }
        }

        public class ClassifiedAdPriceUpdated
        {
            public Guid Id { get; set; }
            public Guid OwnerId { get; set; }
            public decimal Price { get; set; }
            public string CurrencyCode { get; set; }

            public override string ToString()
            {
                return $"{nameof(ClassifiedAdPriceUpdated)}";
            }
        }

        public class ClassifiedAdSentForReview
        {
            public Guid Id { get; set; }
            public Guid OwnerId { get; set; }

            public override string ToString()
            {
                return $"{nameof(ClassifiedAdSentForReview)}";
            }
        }

        public class ClassifiedAdPublished
        {
            public Guid Id { get; set; }
            public Guid OwnerId { get; set; }
            public Guid ApprovedBy { get; set; }

            public override string ToString()
            {
                return $"{nameof(ClassifiedAdPublished)}";
            }
        }

        public class PictureAddedToAClassifiedAd
        {
            public Guid ClassifiedAdId { get; set; }
            public Guid OwnerId { get; set; }
            public Guid PictureId { get; set; }
            public string Url { get; set; }
            public int Height { get; set; }
            public int Width { get; set; }
            public int Order { get; set; }

            public override string ToString()
            {
                return $"{nameof(PictureAddedToAClassifiedAd)}";
            }
        }

        public class ClassifiedAdPictureResized
        {
            public Guid ClassifiedAdId { get; set; }
            public Guid PictureId { get; set; }
            public int Height { get; set; }
            public int Width { get; set; }

            public override string ToString()
            {
                return $"{nameof(ClassifiedAdPictureResized)}";
            }
        }

        public class ClassifiedAdDeleted
        {
            public Guid Id { get; set; }
            public Guid OwnerId { get; set; }

            public override string ToString()
            {
                return $"{nameof(ClassifiedAdDeleted)}";
            }
        }
    }
}