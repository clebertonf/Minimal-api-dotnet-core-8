using RangoAgil.API.EndpointFilters;
using RangoAgil.API.EndpointHandlers;

namespace RangoAgil.API.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static void RegisterRangosEndpoint(this IEndpointRouteBuilder endpoints)
    {
        var baseRangosEndpoints = endpoints.MapGroup("/rangos").RequireAuthorization();
        var baseWhitIdsEnpoints = baseRangosEndpoints.MapGroup("/{id:int}");
        
        // deprecated example
        baseRangosEndpoints.MapGet("/dishes", () => "route deprecated!").WithOpenApi(operation =>
        {
            operation.Deprecated = true;
            return operation;
        }).WithSummary("From 01/02/2025 it will be completely deactivated.")
          .WithDescription($"In version 2.1 it will be completely disabled. For more information send an email with your questions or consult our documentation.\nteamDevs@emial.com");
        
        // group filters (may have performance issues due to chained calls)
        var rangosGroupFilters = baseRangosEndpoints.MapGroup("/{id:int}")
            .AddEndpointFilter(new RangoEndpointFilter(1))
            .AddEndpointFilter(new RangoEndpointFilter(2))
            .RequireAuthorization("")
            .RequireAuthorization("RequierdAdminFromBrazil");

        baseRangosEndpoints.MapGet("", RangosHandlers.GetRangosAsync).AllowAnonymous();
        baseWhitIdsEnpoints.MapGet("", RangosHandlers.GetHangoById).WithName("GetRango");
        baseRangosEndpoints.MapPost("", RangosHandlers.CreateRango).AddEndpointFilter<ValidateAnnotationFilter>();
        rangosGroupFilters.MapPut("", RangosHandlers.UpdateRango);
        rangosGroupFilters.MapDelete("", RangosHandlers.DeleteRango).AddEndpointFilter<LogNotFoundResponseFilter>();
    }

    public static void RegisterIngredientEnpoints(this IEndpointRouteBuilder endpoints)
    {
        var baseIngredients = endpoints.MapGroup("/ingredients").RequireAuthorization();
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