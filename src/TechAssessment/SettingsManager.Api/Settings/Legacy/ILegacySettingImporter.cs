using SettingsManager.Api.Exceptions;
using SettingsManager.Api.Models.Settings.Legacy;
using System.Xml.Serialization;
using System.Xml;
using SettingsManager.Common.Settings;

namespace SettingsManager.Api.Settings.Legacy;

public interface ILegacySettingImporter
{
    ConfigurationFile ParseLegacyConfigurationFile(Stream fileContent);
    ConfigurationImportResult ImportLegacyConfigurationFile(ConfigurationFile legacyConfigurationFile);
}

public class LegacySettingImporter(ISettingsApi settingsApi) : ILegacySettingImporter
{
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
        
        var companySettings = legacyConfigurationFile.GetCompanySettings(ref result);
        var productClientSettings = legacyConfigurationFile.GetProductClientSettings(ref result);
        var emailSettings = legacyConfigurationFile.GetEmailServerSettings(ref result);
        var featureToggles = legacyConfigurationFile.GetFeatureToggles(ref result);
        var customSettings = legacyConfigurationFile.GetCustomSettings(ref result);

        if (result.IsSuccessful)
        {
            settingsApi.SetSettingEntity(companySettings);
            settingsApi.SetSettingEntity(productClientSettings);
            settingsApi.SetSettingEntity(emailSettings);
            foreach (var featureToggle in featureToggles)
            {
                settingsApi.SetSettingValue($"Client.FeatureToggles.{featureToggle.FeatureName}.IsEnabled", SettingDataType.Bool, featureToggle.IsEnabled);
                foreach (var featureToggleAdditionalSetting in featureToggle.AdditionalSettings)
                {
                    settingsApi.SetSettingValue($"Client.FeatureToggles.{featureToggle.FeatureName}.{featureToggleAdditionalSetting.Key}", 
                        featureToggleAdditionalSetting.Value.DataType, 
                        featureToggleAdditionalSetting.Value.Value);
                }
            }

            foreach (var customSetting in customSettings)
            {
                settingsApi.SetSettingValue(customSetting.Key, customSetting.DataType, customSetting.Value);
            }
        }

        return result;
    }
}