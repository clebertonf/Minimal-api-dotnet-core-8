using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using RangoAgil.API.Context;
using RangoAgil.API.ModelsDTO;

namespace RangoAgil.API.EndpointHandlers;

public static class IngredientHandlers
{
    public static async Task<Results<NotFound,Ok<IEnumerable<IngredientDTO>>>>GetIngredientsAsync(ApplicationDbContext context, IMapper mapper, int id)
    {
        var rangos = await context.Ingredients.FirstOrDefaultAsync(x => x.Id == id);
        if(rangos is null)
            return TypedResults.NotFound();
        
        var rangosWithIngredients = (await context.Rangos
            .Include(rango => rango.Ingredients)
            .FirstOrDefaultAsync(rango => rango.Id == id))?.Ingredients;
        
        return TypedResults.Ok(mapper.Map<IEnumerable<IngredientDTO>>(rangosWithIngredients));
    }
}