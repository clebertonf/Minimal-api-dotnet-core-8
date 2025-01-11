using System.Net;

namespace RangoAgil.API.EndpointFilters;

public class LogNotFoundResponseFilter(ILogger<LogNotFoundResponseFilter> logger) : IEndpointFilter
{
    // example of filter for log every time something is deleted and is not found
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var result = await next(context);
        var actualResults = (result is INestedHttpResult result1) ? result1.Result : (IResult)result;
        
        if (actualResults is IStatusCodeHttpResult { StatusCode: (int)HttpStatusCode.NotFound })
        {
            logger.LogInformation($"Resource {context.HttpContext.Request.Path} was not found.");
        }
        return result;
    }
}