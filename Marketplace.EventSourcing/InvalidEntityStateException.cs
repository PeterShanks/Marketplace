using System;
using System.Runtime.Serialization;

namespace Marketplace.EventSourcing;

[Serializable]
public class InvalidEntityStateException : Exception
{
    public InvalidEntityStateException(object entity, string message)
        : base($"Entity {entity.GetType().Name} state change rejected, {message}")
    {
    }

    protected InvalidEntityStateException(
        SerializationInfo info,
        StreamingContext context) : base(info, context)
    {
    }
}