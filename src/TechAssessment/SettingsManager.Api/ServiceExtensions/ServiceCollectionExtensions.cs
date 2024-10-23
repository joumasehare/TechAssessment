using Microsoft.Extensions.DependencyInjection;
using AcmeProduct.Api.Settings;
using AcmeProduct.Api.Settings.Legacy;
using AcmeProduct.Data;

namespace AcmeProduct.Api.ServiceExtensions;

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