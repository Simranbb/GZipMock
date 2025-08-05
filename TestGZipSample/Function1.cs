using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace TestGZipSample;

public class Function1
{
    private readonly ILogger<Function1> _logger;
    private readonly IDateUtils _dateUtils;
    private readonly IGuidGenerator _guidGenerator;
#pragma warning disable IDE0290
    public Function1(ILogger<Function1> logger, IDateUtils dateUtils, IGuidGenerator guidGenerator)
    {
        _logger = logger;
        _dateUtils = dateUtils;
        _guidGenerator = guidGenerator;
    }
#pragma warning restore IDE0290

    [Function("Function1")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        try
        {
            GZipRouteApi routeApi = new(_dateUtils, _guidGenerator);
            string endpoint = "/rmroutes/v2-copy/Routes?locationids=4624,9488,4080,1274,1530,5247,8944,8697,2856,4810,4395,1717,3722,2083,1038,6163,4643,2287,4939,748,391,2294,7226,2617,5033,2108,2194,3978,8858,9503,51703,159703,127703,1496,2008,4241,4369,9112,82503,172103,1174,119703,5644,1003,9123,11103,8993,2006,1362,8409";
            //string endpoint = "/api/v2/Routes?locationids=4624,9488,4080,1274,1530,5247,8944,8697,2856,4810,4395,1717,3722,2083,1038,6163,4643,2287,4939,748,391,2294,7226,2617,5033,2108,2194,3978,8858,9503,51703,159703,127703,1496,2008,4241,4369,9112,82503,172103,1174,119703,5644,1003,9123,11103,8993,2006,1362,8409";
            var response = await routeApi.GetAsync(endpoint);
            return new OkObjectResult(response);
        }
        catch(Exception ex)
        {
            var message = ex.Message;
        }

        return new OkObjectResult("Failure");


    }
}