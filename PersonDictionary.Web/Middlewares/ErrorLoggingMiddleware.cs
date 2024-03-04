using System.Net;
using Newtonsoft.Json;

namespace PersonDictionary.Middlewares;

public class ErrorLoggingMiddleware
{
    public RequestDelegate _requestDelegate;
    private readonly ILogger<ErrorLoggingMiddleware> _logger;

    public ErrorLoggingMiddleware(RequestDelegate requestDelegate, ILogger<ErrorLoggingMiddleware> logger)
    {
        _requestDelegate = requestDelegate;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _requestDelegate(context);
        }
        catch (Exception ex)
        {
            await HandleException(context, ex);
        }
    }

    private Task HandleException(HttpContext context, Exception ex)
    {
        _logger.LogError(ex.ToString());
        var errorMessageObject = new { Message = ex.Message, Code = StatusCodes.Status500InternalServerError };

        var errorMessage = JsonConvert.SerializeObject(errorMessageObject);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        return context.Response.WriteAsync(errorMessage);
    }
}