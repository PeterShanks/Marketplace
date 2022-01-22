using System;
using System.Runtime.Serialization;

namespace Marketplace.EventSourcing;

[Serializable]
public class InvalidValueException : Exception
{
    public InvalidValueException(Type type, string message)
        : base($"Value of {type.Name} {message}")
    {
    }

    protected InvalidValueException(
        SerializationInfo info,
        StreamingContext context) : base(info, context)
    {
    }
}