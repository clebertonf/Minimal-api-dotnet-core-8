using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RangoAgil.API.Context;
using RangoAgil.API.Extensions;
using RangoAgil.API.Profiles;

var builder = WebApplication.CreateBuilder(args);

// Add db context EF core
builder.Services.AddDbContext<ApplicationDbContext>(
    o => o.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
// add automapper
builder.Services.AddAutoMapper(typeof(RangoAgilProfile));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("TokenAuthRango",
        new()
        {
            Name = "Authorization",
            Description = "Token based Authentication and Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            In = ParameterLocation.Header
        }
    );
    options.AddSecurityRequirement(new()
        {
            {
                new ()
                {
                    Reference = new OpenApiReference {
                        Type = ReferenceType.SecurityScheme,
                        Id = "TokenAuthRango" 
                    }
                }, 
                new List<string>()
            }
        }
    );
});

// new configuration exception
builder.Services.AddProblemDetails();

builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization();
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("RequierdAdminFromBrazil",policy =>
    {
        policy.RequireRole("admin");
        policy.RequireClaim("country", "Brazil");
    });

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
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Add register endpoints
app.RegisterRangosEndpoint();
app.RegisterIngredientEnpoints();

app.Run();