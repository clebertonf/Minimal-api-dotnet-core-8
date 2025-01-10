using Microsoft.EntityFrameworkCore;
using RangoAgil.API.Context;
using RangoAgil.API.Extensions;
using RangoAgil.API.Profiles;

var builder = WebApplication.CreateBuilder(args);

// Add db context EF core
builder.Services.AddDbContext<ApplicationDbContext>(
    o => o.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAutoMapper(typeof(RangoAgilProfile));

var app = builder.Build();

// Add register endpoints
app.RegisterRangosEndpoint();
app.RegisterIngredientEnpoints();

app.Run();