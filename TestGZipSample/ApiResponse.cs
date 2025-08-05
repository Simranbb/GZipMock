using System.Net;

namespace TestGZipSample
{
    public class ApiResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Data { get; set; }
        public bool NoUpdateRequired => StatusCode.Equals(HttpStatusCode.NoContent);
    }
}
