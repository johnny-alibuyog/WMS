using System;
using System.Runtime.Serialization;

namespace AmpedBiz.Common.Exceptions
{
    public class ResourceNotFoundException : Exception
    {
        public ResourceNotFoundException() : base() { }

        public ResourceNotFoundException(string message) : base(message) { }

        public ResourceNotFoundException(string message, Exception innerException) : base(message, innerException) { }

        public ResourceNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
