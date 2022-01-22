using Marketplace.EventSourcing;

namespace Marketplace.Ads.Domain.ClassifiedAds;

public class ClassifiedAdText : ValueObject
{
    internal ClassifiedAdText(string text)
    {
        Value = text;
    }

    // Satisfy the serialization requirements 
    protected ClassifiedAdText()
    {
    }

    public string Value { get; }

    public static ClassifiedAdText FromString(string text)
    {
        return new ClassifiedAdText(text);
    }

    public static implicit operator string(ClassifiedAdText text)
    {
        return text.Value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}