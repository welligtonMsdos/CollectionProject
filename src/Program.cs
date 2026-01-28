using Collection10Api.src.Application.Interfaces;
using Collection10Api.src.Application.Validators.Vinyl;
using Collection10Api.src.Infrastructure.Data.Context;
using Collection10Api.src.Infrastructure.Filters;
using Collection10Api.src.Infrastructure.Middleware;
using Collection10Api.src.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.WithOrigins("http://localhost:4200")
        .AllowCredentials()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

// Add services to the container.

builder.Services.AddValidatorsFromAssemblyContaining<VinylCreateValidator>();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var secret = Environment.GetEnvironmentVariable("JwtSettings__Key");

if (string.IsNullOrEmpty(secret) || secret.Length < 32)
{
    // Lança um erro claro para você ver no log do Docker
    throw new InvalidOperationException("A chave JWT está ausente ou é curta demais (mínimo 32 caracteres).");
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
            ValidateIssuer = true,
            ValidIssuer = "http://localhost:5011",
            ValidateAudience = false,
            ValidateLifetime = true
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var connectionString = builder.Configuration.GetConnectionString("CollectionConnection");

builder.Services.AddDbContext<CollectionContext>(options => options.UseNpgsql(connectionString));

builder.Services.Scan(scan =>
{
    scan.FromAssemblyOf<IService>()
    .AddClasses(c => c.AssignableTo<IService>())
    .AsImplementedInterfaces()
    .WithScopedLifetime();

    scan.FromAssemblyOf<IRepository>()
    .AddClasses(c => c.AssignableTo<IRepository>())
    .AsImplementedInterfaces()
    .WithScopedLifetime();
});

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.MapOpenApi();

app.MapScalarApiReference(options =>
{
    options.Title = "Api Authentication";
    options.Theme = ScalarTheme.BluePlanet;
    options.DefaultHttpClient = new(ScalarTarget.JavaScript, ScalarClient.HttpClient);
    options.CustomCss = "";
    options.ShowSidebar = true;
    options.AddPreferredSecuritySchemes("Bearer")
           .AddHttpAuthentication("Bearer", auth =>
           {
               auth.Token = "your-bearer-token";
           });
});
//}

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
