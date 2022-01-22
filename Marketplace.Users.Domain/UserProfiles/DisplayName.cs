using Marketplace.EventSourcing;
using Marketplace.Users.Domain.Shared;

namespace Marketplace.Users.Domain.UserProfiles;

public class DisplayName : ValueObject
{
    internal DisplayName(string displayName)
    {
        Value = displayName;
    }

    // Satisfy the serialization requirements
    protected DisplayName()
    {
    }

    public string Value { get; }

    public static DisplayName FromString(
        string displayName,
        CheckTextForProfanity hasProfanity)
    {
        if (displayName.IsEmpty())
            throw new ArgumentNullException(nameof(displayName));

        if (hasProfanity(displayName).GetAwaiter().GetResult())
            throw new DomainExceptions.ProfanityFoundException(displayName);

        return new DisplayName(displayName);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator string(DisplayName displayName)
    {
        return displayName.Value;
    }
}