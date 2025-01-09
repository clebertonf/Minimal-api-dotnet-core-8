using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RangoAgil.API.Context;
using RangoAgil.API.ModelsDTO;
using RangoAgil.API.Profiles;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>(
    o => o.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAutoMapper(typeof(RangoAgilProfile));

var app = builder.Build();

app.MapGet("/rangos", async Task<Results<NoContent, Ok<IEnumerable<RangoDTO>>>> (
    ApplicationDbContext context, 
    IMapper mapper,
    [FromQuery(Name = "name")]string? name
    ) =>
{
    var rangos = await context.Rangos
        .Where(x => name == null || x.Name.ToLower().Contains(name.ToLower()))
        .ToListAsync();

    if (rangos.Count <= 0)
        return TypedResults.NoContent();
    
    return TypedResults.Ok(mapper.Map<IEnumerable<RangoDTO>>(rangos));
});

app.MapGet("/rango/{id:int}/ingredients", async (
    ApplicationDbContext context,
    IMapper mapper,
    int id) =>
{
    var rango = await context.Rangos
        .Include(r => r.Ingredients)
        .FirstOrDefaultAsync(r => r.Id == id);
    
    return mapper.Map<IEnumerable<RangoDTO>>(rango);
});

app.MapGet("/rango/{id:int}", async (
    ApplicationDbContext context,
    IMapper mapper,
    int id
    ) =>
{
    return mapper.Map<RangoDTO>(await context.Rangos.FirstOrDefaultAsync(r => r.Id == id));
});

app.Run();