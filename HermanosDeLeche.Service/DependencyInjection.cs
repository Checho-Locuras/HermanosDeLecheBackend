using HermanosDeLeche.Domain.Configuration;
using HermanosDeLeche.Domain.Interfaces;
using HermanosDeLeche.Service.Database;
using HermanosDeLeche.Service.Repositories;
using HermanosDeLeche.Service.Security;
using HermanosDeLeche.Service.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HermanosDeLeche.Service;

public static class DependencyInjection
{
    public static IServiceCollection AddHermanosDeLecheServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));

        var connectionString = configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException("ConnectionStrings:Default is not configured.");

        services.AddSingleton<INpgsqlConnectionFactory>(_ => new NpgsqlConnectionFactory(connectionString));

        services.AddScoped<IMilkmanRepository, MilkmanRepository>();
        services.AddScoped<ICowRepository, CowRepository>();
        services.AddScoped<IIntakeRepository, IntakeRepository>();
        services.AddScoped<IStatsRepository, StatsRepository>();

        services.AddSingleton<JwtTokenIssuer>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICowService, CowService>();
        services.AddScoped<IIntakeService, IntakeService>();
        services.AddScoped<IMilkmanService, MilkmanService>();
        services.AddScoped<IStatsService, StatsService>();

        return services;
    }
}
