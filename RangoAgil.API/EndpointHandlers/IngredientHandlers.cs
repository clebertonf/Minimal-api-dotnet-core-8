using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RangoAgil.API.Context;
using RangoAgil.API.Entities;
using RangoAgil.API.ModelsDTO;

namespace RangoAgil.API.EndpointHandlers;

public static class IngredientHandlers
{
    public static async Task<Results<NotFound, Ok<IEnumerable<IngredientDTO>>>> GetIngredientsAsync(ApplicationDbContext context, IMapper mapper)
    {
        var ingredients = await context.Ingredients.ToListAsync();
        if (ingredients is null)
            return TypedResults.NotFound();
        
        return TypedResults.Ok(mapper.Map<IEnumerable<IngredientDTO>>(ingredients));
    }
    
    public static async Task<Results<NotFound, Ok<IngredientDTO>>> GetIngredientByIdAsync(ApplicationDbContext context, IMapper mapper, int id)
    {
        var ingredient = await context.Ingredients.FirstOrDefaultAsync(ingredient => ingredient.Id == id);
        if (ingredient is null)
            return TypedResults.NotFound();
        
        return TypedResults.Ok(mapper.Map<IngredientDTO>(ingredient));
    }

    public static async Task<CreatedAtRoute<IngredientDTO>> CreateIngredient(ApplicationDbContext context, IngredientCreationDTO ingredient, IMapper mapper)
    {
        var ingredientEntity = mapper.Map<Ingredient>(ingredient);
        await context.Ingredients.AddAsync(ingredientEntity);
        await context.SaveChangesAsync();
        var result = mapper.Map<IngredientDTO>(ingredientEntity);
        
        return TypedResults.CreatedAtRoute(result, "GetIngredient", new { id = ingredientEntity.Id });
    }

    public static async Task<Results<NotFound, Ok>> UpdateIngredient(ApplicationDbContext context, IMapper mapper, int id, [FromBody] IngredientUpdate ingredientToUpdate)
    {
        var ingredientEntity = await context.Ingredients.FindAsync(id);
        if (ingredientEntity is null)
            return TypedResults.NotFound();
        
        mapper.Map(ingredientToUpdate, ingredientEntity);
        await context.SaveChangesAsync();
        return TypedResults.Ok();
    }
    
    public static async Task<Results<NoContent, NotFound>> DeleteIngredient(ApplicationDbContext context, IMapper mapper, int id)
    {
        var ingredientEntity = await context.Ingredients.FirstOrDefaultAsync(ingredient => ingredient.Id == id);
        if (ingredientEntity is null)
            return TypedResults.NotFound();
       
        context.Ingredients.Remove(ingredientEntity);
        await context.SaveChangesAsync();
        return TypedResults.NoContent();
    }
    
    public static async Task<Results<NotFound,Ok<IEnumerable<IngredientDTO>>>>GetRangosWhithIngredientsAsync(ApplicationDbContext context, IMapper mapper, int id)
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