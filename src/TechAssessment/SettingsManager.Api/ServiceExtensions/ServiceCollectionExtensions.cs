using Microsoft.Extensions.DependencyInjection;
using SettingsManager.Api.Settings;
using SettingsManager.Api.Settings.Legacy;
using SettingsManager.Data;

namespace SettingsManager.Api.ServiceExtensions;

public static class ServiceCollectionExtensions
{
    public static void AddApiServices(this IServiceCollection services, string connectionString)
    {
        var unitOfWorkBuilder = new UnitOfWorkBuilder();
        
        services.AddScoped<IUnitOfWork>(_ => unitOfWorkBuilder.Build(connectionString));
        services.AddScoped(typeof(IRepository<,>), typeof(BaseRepository<,>));
        services.AddScoped<ISettingsApi, SettingsApi>();
        services.AddScoped<ILegacySettingImporter, LegacySettingImporter>();
    }
}