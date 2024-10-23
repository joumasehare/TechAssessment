using Microsoft.Extensions.DependencyInjection;
using SettingsManager.Api.AutoMapperProfiles;
using SettingsManager.Api.Settings;
using SettingsManager.Api.Settings.Legacy;
using SettingsManager.Data;

namespace SettingsManager.Api.ServiceExtensions;

public static class ServiceCollectionExtensions
{
    public static void AddApiServices(this IServiceCollection services, string connectionString)
    {
        UnitOfWorkBuilder unitOfWorkBuilder = new UnitOfWorkBuilder();
            
        services.AddAutoMapper(typeof(SettingsAutoMapperProfile));
        services.AddScoped<IUnitOfWork>(provider => unitOfWorkBuilder.Build(connectionString));
        services.AddScoped(typeof(IRepository<,>), typeof(BaseRepository<,>));
        services.AddScoped<ISettingsApi, SettingsApi>();
        services.AddScoped<ILegacySettingImporter, LegacySettingImporter>();
    }
}