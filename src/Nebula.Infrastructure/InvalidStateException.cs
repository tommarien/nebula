using System;
using System.Runtime.Serialization;

namespace Nebula.Infrastructure
{
    [Serializable]
    public class InvalidStateException : BusinessException
    {
        public InvalidStateException()
        {
        }

        public InvalidStateException(string message) : base(message)
        {
        }

        protected InvalidStateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public InvalidStateException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}