namespace Marketplace.Ads.Messages;

public static class Commands
{
    public static class V1
    {
        public class Create
        {
            public Guid Id { get; set; }
            public Guid OwnerId { get; set; }

            public override string ToString()
            {
                return $"CreateClassifiedAd {Id}";
            }
        }

        public class ChangeTitle
        {
            public Guid Id { get; set; }
            public string Title { get; set; }

            public override string ToString()
            {
                return $"SetTitle {Id} {Title}";
            }
        }

        public class UpdateText
        {
            public Guid Id { get; set; }
            public string Text { get; set; }

            public override string ToString()
            {
                return $"UpdateText {Id} {Text}";
            }
        }

        public class UpdatePrice
        {
            public Guid Id { get; set; }
            public decimal Price { get; set; }
            public string Currency { get; set; }

            public override string ToString()
            {
                return $"UpdatePrice {Id} {Price}";
            }
        }

        public class RequestToPublish
        {
            public Guid Id { get; set; }

            public override string ToString()
            {
                return $"RequestToPublish {Id}";
            }
        }

        public class Publish
        {
            public Guid Id { get; set; }
            public Guid ApprovedBy { get; set; }

            public override string ToString()
            {
                return $"Publish {Id}";
            }
        }

        public class Delete
        {
            public Guid Id { get; set; }

            public override string ToString()
            {
                return $"Delete {Id}";
            }
        }

        public class UploadImage
        {
            public Guid Id { get; set; }
            public string Image { get; set; }

            public override string ToString()
            {
                return $"Upload image for {Id}";
            }
        }
    }
}