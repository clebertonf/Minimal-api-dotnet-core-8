using MiniValidation;
using RangoAgil.API.ModelsDTO;

namespace RangoAgil.API.EndpointFilters;

public class ValidateAnnotationFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var rangoDtoCreation = context.GetArgument<RangoCreationDTO>(2);

        if (!MiniValidator.TryValidate(rangoDtoCreation, out var validationResult))
        {
            return TypedResults.ValidationProblem(validationResult);
        }
        
        return await next(context);
    }
}