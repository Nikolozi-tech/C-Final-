using HealthcareBillingSystem.Application;
using HealthcareBillingSystem.Infrastructure;
using HealthcareBillingSystem.Infrastructure.Data;
using HealthcareBillingSystem.Presentation.Controllers;
using HealthcareBillingSystem.Presentation.Middleware;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services
    .AddControllers()
    .AddApplicationPart(typeof(AuthController).Assembly);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Healthcare Billing System API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter a valid JWT Bearer token."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseMiddleware<JwtAuthMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

if (app.Configuration.GetValue<bool>("SeedDatabaseOnStartup"))
{
    await DatabaseSeeder.SeedIdentityAsync(app.Services);
}

app.MapControllers();

app.Run();
