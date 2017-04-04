using System;
using System.Runtime.Serialization;

namespace AmpedBiz.Common.Exceptions
{
    public class ResourceAlreadyExistsException : Exception
    {
        public ResourceAlreadyExistsException() : base() { }

        public ResourceAlreadyExistsException(string message) : base(message) { }

        public ResourceAlreadyExistsException(string message, Exception innerException) : base(message, innerException) { }

        public ResourceAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
