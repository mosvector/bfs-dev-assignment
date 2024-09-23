using System;
using System.Runtime.Serialization;

namespace DevAssignment.Exceptions
{
    /// <summary>
    /// Exception thrown when the format is not supported
    /// </summary>
    [Serializable]
    internal class NotSupportedFormatException : Exception
    {
        public string Format { get; }

        public NotSupportedFormatException()
        {
        }

        public NotSupportedFormatException(string message, string format) : base(message)
        {
            Format = format;
        }

        public NotSupportedFormatException(string message, string format, Exception innerException) : base(message, innerException)
        {
            Format = format;
        }

        protected NotSupportedFormatException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}