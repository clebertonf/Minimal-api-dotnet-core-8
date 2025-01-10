using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RangoAgil.API.Context;
using RangoAgil.API.Entities;
using RangoAgil.API.ModelsDTO;
using RangoAgil.API.Profiles;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>(
    o => o.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAutoMapper(typeof(RangoAgilProfile));

var app = builder.Build();

var baseRangosEndpoints = app.MapGroup("/rangos");
var baseWhitIdsEnpoints = baseRangosEndpoints.MapGroup("/{id:int}");
var baseWhitIngredients = baseWhitIdsEnpoints.MapGroup("/ingredients");

baseRangosEndpoints.MapGet("", async Task<Results<NoContent, Ok<IEnumerable<RangoDTO>>>> (
    ApplicationDbContext context, 
    IMapper mapper,
    [FromQuery(Name = "name")]string? name
    ) =>
{
    var rangosEntity = await context.Rangos
        .Where(x => name == null || x.Name.ToLower().Contains(name.ToLower()))
        .ToListAsync();

    if (rangosEntity.Count <= 0)
        return TypedResults.NoContent();
    
    return TypedResults.Ok(mapper.Map<IEnumerable<RangoDTO>>(rangosEntity));
});

baseWhitIngredients.MapGet("", async (
    ApplicationDbContext context,
    IMapper mapper,
    int id) =>
{
    var rangoEntity = await context.Rangos
        .Include(r => r.Ingredients)
        .FirstOrDefaultAsync(r => r.Id == id);
    
    return mapper.Map<IEnumerable<RangoDTO>>(rangoEntity);
});

baseWhitIdsEnpoints.MapGet("", async (
    ApplicationDbContext context,
    IMapper mapper,
    int id
    ) =>
{
    return mapper.Map<RangoDTO>(await context.Rangos.FirstOrDefaultAsync(r => r.Id == id));
}).WithName("GetRango");

baseRangosEndpoints.MapPost("", async (
    ApplicationDbContext context,
    IMapper mapper,
    [FromBody] RangoCreationDTO rangoCreation
   // LinkGenerator linkGenerator,
   // HttpContext httpContext
    ) =>
{
    var rangoEntity = mapper.Map<Rango>(rangoCreation);
    context.Rangos.Add(rangoEntity);
    await context.SaveChangesAsync();
    var result = mapper.Map<RangoDTO>(rangoEntity);

   /* var linkToReturn = linkGenerator.GetUriByName(httpContext, "GetRango", new { id = rangoEntity.Id });
    return TypedResults.Created(linkToReturn, result);
    */
   
   return TypedResults.CreatedAtRoute(result, "GetRango", new { id = rangoEntity.Id });
});

baseWhitIdsEnpoints.MapPut("", async Task<Results<NotFound, Ok>>(
    ApplicationDbContext context,
    IMapper mapper,
    int id,
    [FromBody] RangoUpdateDTO rangoUpdate) =>
{
    var rangoEntity = await context.Rangos.FirstOrDefaultAsync(r => r.Id == id);
    if (rangoEntity is null)
        return TypedResults.NotFound();
    
    mapper.Map(rangoUpdate, rangoEntity);
    await context.SaveChangesAsync();
    
    return TypedResults.Ok(); 
});

baseWhitIdsEnpoints.MapDelete("", async Task<Results<NotFound, Ok>>(
    ApplicationDbContext context,
    int id) =>
{
    var rangoEntity = await context.Rangos.FirstOrDefaultAsync(r => r.Id == id);
    if (rangoEntity is null)
        return TypedResults.NotFound();
    
    context.Rangos.Remove(rangoEntity);
    await context.SaveChangesAsync();
    
    return TypedResults.Ok(); 
});

app.Run();