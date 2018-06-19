using System;
using System.Runtime.Serialization;

namespace Utility
{
    [Serializable]
    public class CommonException : ApplicationException
    {
        public CommonException()
        {
        }

        public CommonException(string message) : base(message)
        {
        }

        public CommonException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CommonException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}