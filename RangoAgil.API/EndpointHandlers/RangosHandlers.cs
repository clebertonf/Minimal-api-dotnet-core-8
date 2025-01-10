using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RangoAgil.API.Context;
using RangoAgil.API.Entities;
using RangoAgil.API.ModelsDTO;

namespace RangoAgil.API.EndpointHandlers;

public static class RangosHandlers
{
    public static async Task<Results<NoContent, Ok<IEnumerable<RangoDTO>>>> GetRangosAsync (ApplicationDbContext context, IMapper mapper, [FromQuery(Name = "name")]string? name)
    {
        var rangosEntity = await context.Rangos
            .Where(x => name == null || x.Name.ToLower().Contains(name.ToLower()))
            .ToListAsync();

        if (rangosEntity.Count <= 0)
            return TypedResults.NoContent();
    
        return TypedResults.Ok(mapper.Map<IEnumerable<RangoDTO>>(rangosEntity));
    }
    
    public static async Task<Results<NotFound, Ok<RangoDTO>>> GetHangoById(ApplicationDbContext context, IMapper mapper, int id)
    {
        var rango = await context.Rangos.FirstOrDefaultAsync(r => r.Id == id);
        if (rango is null)
            return TypedResults.NotFound();
        
        return TypedResults.Ok(mapper.Map<RangoDTO>(rango));
    }
    
    public static async Task<CreatedAtRoute<RangoDTO>> CreateRango(ApplicationDbContext context, IMapper mapper, [FromBody] RangoCreationDTO rangoCreation /* LinkGenerator linkGenerator,// HttpContext httpContext*/ )
    {
        var rangoEntity = mapper.Map<Rango>(rangoCreation);
        context.Rangos.Add(rangoEntity);
        await context.SaveChangesAsync();
        var result = mapper.Map<RangoDTO>(rangoEntity);

        /* var linkToReturn = linkGenerator.GetUriByName(httpContext, "GetRango", new { id = rangoEntity.Id });
         return TypedResults.Created(linkToReturn, result);
         */
   
        return TypedResults.CreatedAtRoute(result, "GetRango", new { id = rangoEntity.Id });
    }
    
    public static async Task<Results<NotFound, Ok>> UpdateRango(ApplicationDbContext context, IMapper mapper, int id, [FromBody] RangoUpdateDTO rangoUpdate)
    {
        var rangoEntity = await context.Rangos.FirstOrDefaultAsync(r => r.Id == id);
        if (rangoEntity is null)
            return TypedResults.NotFound();
    
        mapper.Map(rangoUpdate, rangoEntity);
        await context.SaveChangesAsync();
    
        return TypedResults.Ok(); 
    }
    
    public static async Task<Results<NotFound, Ok>> DeleteRango(ApplicationDbContext context, int id)
    {
        var rangoEntity = await context.Rangos.FirstOrDefaultAsync(r => r.Id == id);
        if (rangoEntity is null)
            return TypedResults.NotFound();
    
        context.Rangos.Remove(rangoEntity);
        await context.SaveChangesAsync();
    
        return TypedResults.Ok(); 
    }
}