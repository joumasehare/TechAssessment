using SettingsManager.Api.Exceptions;
using SettingsManager.Api.Models.Settings;
using SettingsManager.Api.Models.Settings.Legacy;
using System.Xml.Serialization;
using System.Xml;

namespace SettingsManager.Api.Settings.Legacy;

public interface ILegacySettingImporter
{
    ConfigurationFile ParseLegacyConfigurationFile(Stream fileContent);
    ConfigurationImportResult ImportLegacyConfigurationFile(ConfigurationFile legacyConfigurationFile);
}

public class LegacySettingImporter(ISettingsApi settingsApi) : ILegacySettingImporter
{
    private readonly ISettingsApi settingsApi = settingsApi;

    public ConfigurationFile ParseLegacyConfigurationFile(Stream fileStream)
    {
        var serializer = new XmlSerializer(typeof(ConfigurationFile));
        try
        {
            using var reader = XmlReader.Create(fileStream);
            if (!serializer.CanDeserialize(reader))
                throw new ConfigurationFileSerializationException("Xml schema is malformed");

            return (ConfigurationFile)serializer.Deserialize(reader)!;
        }
        catch (XmlException xmlException)
        {
            throw new ConfigurationFileSerializationException(xmlException);
        }
    }

    public ConfigurationImportResult ImportLegacyConfigurationFile(ConfigurationFile legacyConfigurationFile)
    {
        ConfigurationImportResult result = new ConfigurationImportResult();

        /*CompanySettings settings = new CompanySettings();
        legacyConfigurationFile.GetCompanySettings(result, settings);
        legacyConfigurationFile.ProductClientSettings(result, settings.ProductClientSettings);
        legacyConfigurationFile.FeatureToggles(result, settings);*/

        CompanySettings companySettings = legacyConfigurationFile.GetCompanySettings(ref result);

        if (result.IsSuccessful)
        {
            settingsApi.SaveSettingEntity(companySettings);
        }

        //Save Concrete Settings
        //settings.MapToDomain()

        //Custom here
        //EmailServerConfiguration emailServerConfiguration = new EmailServerConfiguration();

        return result;
    }
}