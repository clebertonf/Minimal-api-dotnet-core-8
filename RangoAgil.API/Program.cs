using System.Net;
using Microsoft.EntityFrameworkCore;
using RangoAgil.API.Context;
using RangoAgil.API.Extensions;
using RangoAgil.API.Profiles;

var builder = WebApplication.CreateBuilder(args);

// Add db context EF core
builder.Services.AddDbContext<ApplicationDbContext>(
    o => o.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
// add automapper
builder.Services.AddAutoMapper(typeof(RangoAgilProfile));

// new configuration exception
builder.Services.AddProblemDetails();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
    
    // old custom exception
    /* app.UseExceptionHandler(configurationBuilder =>
    {
        configurationBuilder.Run(
            async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "text/html";
                await context.Response.WriteAsync("<h1>An unexpected problem happened.</h1>");
            });
    });
    */
}

// Add register endpoints
app.RegisterRangosEndpoint();
app.RegisterIngredientEnpoints();

app.Run();