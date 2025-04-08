﻿namespace RentIt.Domain.Common;
public abstract class ValueObject
{
    protected abstract IEnumerable<object?> GetEqualityComponents();

    public override bool Equals(object? obj)
    {
        if (obj is not ValueObject other || GetType() != other.GetType())
            return false;

        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Aggregate(1, (hash, obj) =>
            {
                unchecked
                {
                    return hash * 23 + (obj?.GetHashCode() ?? 0);
                }
            });
    }
}
