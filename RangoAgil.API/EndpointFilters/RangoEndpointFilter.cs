namespace RangoAgil.API.EndpointFilters;

public class RangoEndpointFilter : IEndpointFilter
{
    private readonly int _lockedId;

    public RangoEndpointFilter(int lockedId)
    {
        _lockedId = lockedId;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var id = context.GetArgument<int>(1);
        if (id == _lockedId)
        {
            return TypedResults.Problem(
                new()
                {
                    Status = 400,
                    Title = "Unable to perform the operation",
                    Detail = "It is not possible to change or delete this rango",
                });
        }
        return await next.Invoke(context);
    }
}