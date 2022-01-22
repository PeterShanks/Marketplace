namespace Marketplace.Ads.Domain.Shared;

public static class DomainExceptions
{
    public class InvalidEntityStateException : Exception
    {
        public InvalidEntityStateException(object entity, string message)
            : base($"Entity {entity.GetType().Name} state change rejected, {message}")
        {
        }
    }

    public class ProfanityFoundException : Exception
    {
        public ProfanityFoundException(string text)
            : base($"Profanity found in text: {text}")
        {
        }
    }

    public class CurrencyMismatchException : Exception
    {
        public CurrencyMismatchException(string message) : base(message)
        {
        }
    }
}