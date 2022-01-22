using System;

namespace Marketplace.Framework
{

    [Serializable]
    public class InvalidValueException : Exception
    {
        public InvalidValueException(Type type, string message)
            : base($"Value of {type.Name} {message}")
        {
        }
        protected InvalidValueException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
