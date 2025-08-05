using System;
using System.Net;
using System.Runtime.Serialization;

namespace TestGZipSample
{

    [Serializable]
    public class DownloadFailedException : Exception
    {
        public HttpStatusCode? StatusCode { get; }

        public DownloadFailedException() : base()
        {
        }

        public DownloadFailedException(string message) : base(message)
        {
        }

        public DownloadFailedException(string message, Exception inner) : base(message, inner)
        {
        }

        public DownloadFailedException(string exception, HttpStatusCode httpStatusCode) : base(exception)
        {
            StatusCode = httpStatusCode;
        }

        protected DownloadFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
