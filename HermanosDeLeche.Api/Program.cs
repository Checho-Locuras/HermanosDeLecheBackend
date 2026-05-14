using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using HermanosDeLeche.Api.Middleware;
using HermanosDeLeche.Domain.Configuration;
using HermanosDeLeche.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var railwayPort = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrWhiteSpace(railwayPort))
    builder.WebHost.UseUrls($"http://0.0.0.0:{railwayPort}");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Hermanos de Leche API",
        Version = "v1",
        Description = "API rural-social con humor: lecheros, vacas e ingestas de leche."
    });

    var jwtScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description =
            "Pega solo el JWT (sin la palabra Bearer). Tras login/register copia el valor de \"token\"."
    };
    const string bearerId = "Bearer";
    options.AddSecurityDefinition(bearerId, jwtScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = bearerId }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddHermanosDeLecheServices(builder.Configuration);

var jwtSettings = builder.Configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>()
    ?? throw new InvalidOperationException("Jwt configuration is missing.");

var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = signingKey,
            ClockSkew = TimeSpan.FromMinutes(2),
            NameClaimType = JwtRegisteredClaimNames.Sub
        };
        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                if (context.Principal?.Identity is not ClaimsIdentity id)
                    return Task.CompletedTask;

                var hasSub = id.HasClaim(static c => c.Type == JwtRegisteredClaimNames.Sub || c.Type == "sub");
                if (hasSub)
                    return Task.CompletedTask;

                var sub = context.SecurityToken switch
                {
                    JwtSecurityToken j => j.Subject,
                    Microsoft.IdentityModel.JsonWebTokens.JsonWebToken jw => jw.Subject,
                    _ => null
                };

                if (!string.IsNullOrEmpty(sub))
                    id.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, sub));

                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Open", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Hermanos de Leche v1");
});

app.UseMiddleware<ExceptionMiddleware>();

app.UseCors("Open");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }))
    .WithTags("Health");

app.Run();
