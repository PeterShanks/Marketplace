using System;
using System.Collections.Generic;
using System.Linq;

namespace Marketplace.EventSourcing;

public abstract class ValueObject : IEquatable<ValueObject>
{
    public bool Equals(ValueObject other)
    {
        if (other is null || other.GetType() != GetType())
            return false;

        return GetEqualityComponents()
            .SequenceEqual(other.GetEqualityComponents());
    }

    public static bool operator ==(ValueObject left, ValueObject right)
    {
        if (left is null ^ right is null)
            return false;

        return left is null || left.Equals(right);
    }

    public static bool operator !=(ValueObject left, ValueObject right)
    {
        return !(left == right);
    }

    protected abstract IEnumerable<object> GetEqualityComponents();

    public override bool Equals(object obj)
    {
        return Equals(obj as ValueObject);
    }

    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Select(x => x?.GetHashCode() ?? 0)
            .Aggregate((x, y) => x ^ y);
    }
}