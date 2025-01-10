using RangoAgil.API.EndpointHandlers;

namespace RangoAgil.API.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static void RegisterRangosEndpoint(this IEndpointRouteBuilder endpoints)
    {
        var baseRangosEndpoints = endpoints.MapGroup("/rangos");
        var baseWhitIdsEnpoints = baseRangosEndpoints.MapGroup("/{id:int}");
        
        baseRangosEndpoints.MapGet("", RangosHandlers.GetRangosAsync);
        baseWhitIdsEnpoints.MapGet("", RangosHandlers.GetHangoById).WithName("GetRango");
        baseRangosEndpoints.MapPost("", RangosHandlers.CreateRango);
        baseWhitIdsEnpoints.MapPut("", RangosHandlers.UpdateRango);
        baseWhitIdsEnpoints.MapDelete("", RangosHandlers.DeleteRango);
    }

    public static void RegisterIngredientEnpoints(this IEndpointRouteBuilder endpoints)
    {
        var baseIngredients = endpoints.MapGroup("/ingredients");
        var baseIngredientsWithId = baseIngredients.MapGroup("/{id:int}");
        
        baseIngredients.MapGet("", IngredientHandlers.GetIngredientsAsync);
        baseIngredientsWithId.MapGet("", IngredientHandlers.GetIngredientByIdAsync).WithName("GetIngredient");
        baseIngredients.MapPost("", IngredientHandlers.CreateIngredient);
        baseIngredientsWithId.MapPut("", IngredientHandlers.UpdateIngredient);
        baseIngredientsWithId.MapDelete("", IngredientHandlers.DeleteIngredient);
        
        var baseWhitIngredients = endpoints.MapGroup("/rangos/{id:int}/ingredients");
        baseWhitIngredients.MapGet("", IngredientHandlers.GetRangosWhithIngredientsAsync);
    }
}