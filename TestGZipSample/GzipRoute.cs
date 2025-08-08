using static System.Net.WebRequestMethods;

namespace TestGZipSample;
public class GZipRouteApi : ApiBase
{
    private const string XRmgLanguage = "X-RMG-Language";
    private const string XRmgDateTime = "X-RMG-Date-Time";
    private const string XRmgClientId = "X-Rmg-Client-Id";
    private const string AcceptEncoding = "Accept-Encoding";
    private const string XRmgLanguageValue = "en";
    private const string AcceptEncodingValue = "gzip";
    private const string BaseUrl = "https://pdaapimqa.royalmailgroup.net";
   // private const string BaseUrl = "https://rmg-we-pda-d-fa-routesmasterdata-01.azurewebsites.net";
    private readonly IDateUtils _dateUtils;
    private readonly IGuidGenerator _guidGenerator;


#pragma warning disable IDE0290
    public GZipRouteApi(
        IDateUtils dateUtils,
        IGuidGenerator guidGenerator)
    {
        _dateUtils = dateUtils;
        _guidGenerator = guidGenerator;
    }
#pragma warning restore IDE0290

    public async Task<ApiResponse> GetAsync(string endPoint)
    {
        var baseUrl = BaseUrl;

        var uri = new UriBuilder($"{baseUrl}{endPoint}").Uri;

        var headers = new Dictionary<string, string>
        {
            //{ XRmgLanguage, XRmgLanguageValue },
            //{ XRmgDateTime, _dateUtils.GetBritishTimeWithOffset(DateTime.UtcNow) },
            //{ XRmgClientId, _guidGenerator.GetNewGuid().ToString() },
            //{ AcceptEncoding, AcceptEncodingValue }
        };

        var httpMessage = GetBaseRequest(uri, headers);

        return await Get(httpMessage);
    }

    public async Task<ApiResponse> PostAsync(string endPoint, string requestBody,
        Dictionary<string, string> endPointParameters = null)
    {
        var baseUrl = "";

        var uri = GetUri(baseUrl, endPoint, endPointParameters);

        var headers = new Dictionary<string, string>
        {
            { XRmgLanguage, XRmgLanguageValue },
            { XRmgDateTime, _dateUtils.GetBritishTimeWithOffset(DateTime.UtcNow) },
            { XRmgClientId, _guidGenerator.GetNewGuid().ToString() },
            { AcceptEncoding, AcceptEncodingValue }
        };

        var httpMessage = PostBaseRequest(uri, requestBody, headers);

        return await Get(httpMessage);
    }

    public async Task<ApiResponse> GetAsync(string endPoint, Dictionary<string, string> endPointParameters)
    {
        var baseUrl = "";

        var uri = GetUri(baseUrl, endPoint, endPointParameters);

        var headers = new Dictionary<string, string>
        {
            { XRmgLanguage, XRmgLanguageValue },
            { XRmgDateTime, _dateUtils.GetBritishTimeWithOffset(DateTime.UtcNow) },
            { XRmgClientId, _guidGenerator.GetNewGuid().ToString() },
            { AcceptEncoding, AcceptEncodingValue }
        };

        var httpMessage = GetBaseRequest(uri, headers);

        return await Get(httpMessage);
    }
}
