using System;

namespace Marketplace.EventSourcing;

public abstract class TypedId : IEquatable<TypedId>
{
    protected TypedId(Guid value)
    {
        if (value == default)
            throw new InvalidOperationException("Id value cannot be empty");

        Value = value;
    }

    public Guid Value { get; }

    public bool Equals(TypedId other)
    {
        return Value == other?.Value;
    }

    public override bool Equals(object obj)
    {
        if (obj is null)
            return false;

        return obj is TypedId other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static bool operator ==(TypedId left, TypedId right)
    {
        if (left is null ^ right is null)
            return false;

        return left is not null && left.Equals(right);
    }

    public static bool operator !=(TypedId left, TypedId right)
    {
        return !(left == right);
    }

    public static implicit operator Guid(TypedId typedId)
    {
        return typedId.Value;
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}