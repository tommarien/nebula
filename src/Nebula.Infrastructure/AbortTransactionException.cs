using System;
using System.Runtime.Serialization;

namespace Nebula.Infrastructure
{
    [Serializable]
    public class AbortTransactionException : ApplicationException
    {
        public AbortTransactionException()
        {
        }

        public AbortTransactionException(string message) : base(message)
        {
        }

        public AbortTransactionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AbortTransactionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}