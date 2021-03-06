using Marketplace.EventSourcing;

namespace Marketplace.Users.Domain.UserProfiles;

public class FullName : ValueObject
{
    internal FullName(string value)
    {
        Value = value;
    }

    // Satisfy the serialization requirements
    protected FullName()
    {
    }

    public string Value { get; }

    public static FullName FromString(string fullName)
    {
        if (fullName.IsEmpty())
            throw new ArgumentNullException(nameof(fullName));

        return new FullName(fullName);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator string(FullName fullName)
    {
        return fullName.Value;
    }
}