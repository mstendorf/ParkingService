using System.Net;
using System.Runtime.Serialization;

namespace CarTypeService.Services
{
    [Serializable]
    internal class MotorApiException : Exception
    {
        private HttpStatusCode StatusCode { get; }
        private HttpContent Content { get; }

        public MotorApiException()
        {
        }

        public MotorApiException(string? message) : base(message)
        {
        }

        public MotorApiException(HttpStatusCode statusCode, HttpContent content)
        {
            this.StatusCode = statusCode;
            this.Content = content;
        }

        public MotorApiException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected MotorApiException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}