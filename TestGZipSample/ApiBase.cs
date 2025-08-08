using System.Collections;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;



namespace TestGZipSample
{
    public class ApiBase
    {
        private const string Accept = "Accept";
        private const string AcceptValue = "application/json";
        private const string Authorization = "Authorization";
        private const string RMGMessageId = "X-RMG-MESSAGE-ID";
        private const string DeviceId = "X-RMG-DEVICE-ID";
        private const string UserName = "X-RMG-USERNAME";
        private const string AcceptEncoding = "Accept-Encoding";
        private const string AcceptEncodingValue = "gzip";
        private readonly HttpClient _client;
        internal readonly HttpClientHandler _handler;


        protected ApiBase()
        {
            _handler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
            };
            _client ??= new HttpClient(_handler)
            {
                 Timeout = TimeSpan.FromSeconds(1000)
            };
        }

        protected HttpRequestMessage GetBaseRequest(Uri uri, IDictionary<string, string> headers = null)
        {
            var requestMessage = new HttpRequestMessage
            {
                RequestUri = uri,
                Method = HttpMethod.Get
            };

            requestMessage.Headers.Add(RMGMessageId, Guid.NewGuid().ToString());
            requestMessage.Headers.Add(DeviceId,"6876786");
            requestMessage.Headers.Add(UserName, "Sim");
            requestMessage.Headers.Add(AcceptEncoding, AcceptEncodingValue);
          //  requestMessage.Headers.Add("X-RMG-CLIENT-ID", "973fd29c-08c2-4f0a-a974-80188c9da8b1");

            //if (CoreAppData.Instance.AccessToken != null)
            //{
            //    requestMessage.Headers.Add(Authorization, $"Bearer {CoreAppData.Instance.AccessToken}");
            //}

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    requestMessage.Headers.Add(header.Key, header.Value);
                }
            }

            return requestMessage;
        }

        protected async Task<ApiResponse> Get(HttpRequestMessage requestMessage)
        {
            var statusCode = HttpStatusCode.RequestTimeout;

            try
            {
                var httpResponse = await _client.SendAsync(requestMessage);
                statusCode = httpResponse.StatusCode;
                if (statusCode == HttpStatusCode.OK && httpResponse.Headers.GetValues("Content-Encoding").Contains("gzip"))
                {
                    return new ApiResponse
                    {
                        Data = httpResponse.IsSuccessStatusCode && !statusCode.Equals(HttpStatusCode.NoContent)
                        ? UncompressData(await httpResponse.Content.ReadAsStringAsync())
                        : string.Empty,
                        StatusCode = statusCode
                    };
                }
                else
                {
                    return new ApiResponse
                    {
                        Data = httpResponse.IsSuccessStatusCode && !statusCode.Equals(HttpStatusCode.NoContent)
                       ? await httpResponse.Content.ReadAsStringAsync()
                       : string.Empty,
                        StatusCode = statusCode
                    };
                }
            }
            catch (Exception exception)
            {
                throw new DownloadFailedException(exception.Message, statusCode);
            }
        }

        public static string UncompressData(string data)
        {
            byte[] compressedData = System.Text.Encoding.UTF8.GetBytes(data);
            string uncompressedData;

            using (var compressedStream = new MemoryStream(compressedData))
            using (var uncompressedStream = new MemoryStream())
            {
                using (GZipStream decompressionStream = new(compressedStream, CompressionMode.Decompress))
                {
                    try
                    {
                        decompressionStream.CopyTo(uncompressedStream);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                using StreamReader reader = new(uncompressedStream, Encoding.UTF8);
                uncompressedData = reader.ReadToEnd(); // Read the stream to a string
            }

            return uncompressedData;
        }

        protected static Uri GetUri(string baseUri, string endpoint, Dictionary<string, string> parameters = null)
        {
            var builder = new UriBuilder($"{baseUri}{endpoint}");

            if (parameters == null)
            {
                return builder.Uri;
            }

            var encodedContent = new FormUrlEncodedContent(parameters);
            builder.Query = encodedContent.ReadAsStringAsync().Result;
            return builder.Uri;
        }

        protected static HttpRequestMessage GetHttpPostRequestMessage(string endPoint, string requestBody)
        {
            var token = "XXX";
            var requestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(endPoint)
            };
            requestMessage.Headers.Add(Authorization, $"Bearer {token}");
            requestMessage.Headers.Add(RMGMessageId, Guid.NewGuid().ToString());
            requestMessage.Headers.Add(DeviceId, "68768768");
            requestMessage.Headers.Add(UserName, "Sim");
            requestMessage.Headers.Add(Accept, AcceptValue);
            requestMessage.Content = new StringContent(requestBody);

            return requestMessage;
        }

        protected static HttpRequestMessage PostBaseRequest(Uri uri, string requestBody, IDictionary<string, string> headers = null)
        {
            var requestMessage = new HttpRequestMessage
            {
                RequestUri = uri,
                Method = HttpMethod.Post
            };

            requestMessage.Headers.Add(RMGMessageId, Guid.NewGuid().ToString());
            requestMessage.Headers.Add(DeviceId, "68768768");
            requestMessage.Headers.Add(UserName, "Sim");
            requestMessage.Headers.Add(Accept, AcceptValue);
            requestMessage.Content = new StringContent(requestBody);

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    requestMessage.Headers.Add(header.Key, header.Value);
                }
            }

            return requestMessage;
        }

        protected async Task<ApiResponse> GetAsync(HttpRequestMessage requestMessage, CancellationToken cancellationToken)
        {
            var statusCode = HttpStatusCode.RequestTimeout;

            try
            {
                var httpResponse = await _client.SendAsync(requestMessage, cancellationToken);
                statusCode = httpResponse.StatusCode;
                httpResponse.EnsureSuccessStatusCode();

                return new ApiResponse
                {
                    Data = httpResponse.IsSuccessStatusCode && !statusCode.Equals(HttpStatusCode.NoContent)
                    ? await httpResponse.Content.ReadAsStringAsync(cancellationToken)
                    : string.Empty,
                    StatusCode = statusCode
                };
            }
            catch (Exception exception)
            {
                throw new DownloadFailedException(exception.Message, statusCode);
            }
        }
    }
}
