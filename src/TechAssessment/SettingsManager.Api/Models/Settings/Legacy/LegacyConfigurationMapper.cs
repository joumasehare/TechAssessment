using SettingsManager.Common.Settings;

namespace SettingsManager.Api.Models.Settings.Legacy;

internal static class LegacyConfigurationMapper
{
    private static readonly Dictionary<Type, Func<string, object>> ValueConverters = new()
    {
        {typeof(string), s => s},
        {typeof(int), s => Convert.ToInt32(s)},
        {typeof(bool), s => Convert.ToBoolean(s.ToLower())},
        {typeof(Uri), s => new Uri(s)},
    };

    internal static CompanySettings GetCompanySettings(this ConfigurationFile config, ref ConfigurationImportResult importResult)
    {
        var result = new CompanySettings
        {
            CompanyName = ParseValue<string>(
                config.AppSettings.AppSettingsKeyValuePairs.Find(k => k.Key == "Company")?.Value,
                ref importResult,
                "CompanySettings",
                "Company"),
            License = ParseValue<string>(
                config.AppSettings.AppSettingsKeyValuePairs.Find(k => k.Key == "License")?.Value,
                ref importResult,
                "CompanySettings",
                "License"),
            ServerAddress = ParseValue<Uri>(
                config.AppSettings.AppSettingsKeyValuePairs.Find(k => k.Key == "ServerAddress")?.Value,
                ref importResult,
                "CompanySettings",
                "ServerAddress"),
            LoginAttemptLimit = ParseValue<int>(
                config.AppSettings.AppSettingsKeyValuePairs.Find(k => k.Key == "LoginAttemptLimit")?.Value,
                ref importResult,
                "CompanySettings",
                "LoginAttemptLimit"),
            PasswordHistory = ParseValue<int>(
                config.AppSettings.AppSettingsKeyValuePairs.Find(k => k.Key == "PasswordHistory")?.Value,
                ref importResult,
                "CompanySettings",
                "PasswordHistory")
        };

        return result;
    }

    internal static ProductClientSettings GetProductClientSettings(this ConfigurationFile config, ref ConfigurationImportResult importResult)
    {
        var result = new ProductClientSettings
        {
            WebsiteUrl = ParseValue<Uri>(
                config.AppSettings.AppSettingsKeyValuePairs.Find(k => k.Key == "WebsiteUrl")?.Value,
                ref importResult,
                "ProductClientSettings",
                "WebsiteUrl"),
            ShowHeader = ParseValue<bool>(
                config.AppSettings.AppSettingsKeyValuePairs.Find(k => k.Key == "ShowHeader")?.Value,
                ref importResult,
                "ProductClientSettings",
                "ShowHeader"),
            ShowMenuBar = ParseValue<bool>(
                config.AppSettings.AppSettingsKeyValuePairs.Find(k => k.Key == "ShowMenuBar")?.Value,
                ref importResult,
                "ProductClientSettings",
                "ShowMenuBar"),
            SubMenusToShow = ParseValue<int>(
                config.AppSettings.AppSettingsKeyValuePairs.Find(k => k.Key == "SubMenusToShow")?.Value,
                ref importResult,
                "ProductClientSettings",
                "SubMenusToShow"),
            DisplayFullError = ParseValue<bool>(
                config.AppSettings.AppSettingsKeyValuePairs.Find(k => k.Key == "DisplayFullError")?.Value,
                ref importResult,
                "ProductClientSettings",
                "DisplayFullError")
        };

        return result;
    }

    internal static List<FeatureToggle> GetFeatureToggles(this ConfigurationFile config, ref ConfigurationImportResult importResult)
    {
        var result = new List<FeatureToggle>()
        {
            new()
            {
                FeatureName = "Feature1",
                IsEnabled = ParseValue<bool>(
                    config.AppSettings.AppSettingsKeyValuePairs.Find(k => k.Key == "EnableFeature1")?.Value,
                    ref importResult,
                    "FeatureToggles",
                    "EnableFeature1"),
                AdditionalSettings = new Dictionary<string, (SettingDataType DataType, object Value)>
                {
                    {
                        "AdvancedSearch", (SettingDataType.Bool, ParseValue<bool>(
                            config.AppSettings.AppSettingsKeyValuePairs.Find(k => k.Key == "EnableFeature1AdvancedSearch")?.Value,
                            ref importResult,
                            "FeatureToggles",
                            "EnableFeature1AdvancedSearch"))
                    }
                }
            },
            new()
            {
                FeatureName = "Feature2",
                IsEnabled = ParseValue<bool>(
                    config.AppSettings.AppSettingsKeyValuePairs.Find(k => k.Key == "EnableFeature2")?.Value,
                    ref importResult,
                    "FeatureToggles",
                    "EnableFeature2")
            },
            new()
            {
                FeatureName = "Feature3",
                IsEnabled = ParseValue<bool>(
                    config.AppSettings.AppSettingsKeyValuePairs.Find(k => k.Key == "EnableFeature3")?.Value,
                    ref importResult,
                    "FeatureToggles",
                    "EnableFeature3"),
                AdditionalSettings = new Dictionary<string, (SettingDataType DataType, object Value)>
                {
                    {
                        "AdvancedSearch", (SettingDataType.Bool, ParseValue<bool>(
                            config.AppSettings.AppSettingsKeyValuePairs.Find(k => k.Key == "EnableFeature3AdvancedSearch")?.Value,
                            ref importResult,
                            "FeatureToggles",
                            "EnableFeature3AdvancedSearch"))
                    },
                    {
                        "ItemCount", (SettingDataType.Int, ParseValue<int>(
                            config.AppSettings.AppSettingsKeyValuePairs.Find(k => k.Key == "Feature3ItemCount")?.Value,
                            ref importResult,
                            "FeatureToggles",
                            "Feature3ItemCount"))
                    }
                }
            },
            new()
            {
                FeatureName = "Feature4",
                IsEnabled = ParseValue<bool>(
                    config.AppSettings.AppSettingsKeyValuePairs.Find(k => k.Key == "EnableFeature4")?.Value,
                    ref importResult,
                    "FeatureToggles",
                    "EnableFeature4")
            },
            new()
            {
                FeatureName = "Feature5",
                IsEnabled = ParseValue<bool>(
                    config.AppSettings.AppSettingsKeyValuePairs.Find(k => k.Key == "EnableFeature5")?.Value,
                    ref importResult,
                    "FeatureToggles",
                    "EnableFeature5")
            }
        };

        return result;
    }

    internal static EmailConfiguration? GetEmailServerSettings(this ConfigurationFile config, ref ConfigurationImportResult importResult)
    {
        var emailSettings = config.CustomSettings.Keys.Find(k => k.Key == "Email");
        var serverSettings = emailSettings.Keys.Find(s => s.Key == "Server");
        var messageSettings = emailSettings.Keys.Find(s => s.Key == "Message");

        var serverConfiguration = new EmailServerConfiguration
        {
            HostAddress = ParseValue<string>(
                serverSettings.Values.Find(s => s.Key == "HostAddress")?.Value,
                ref importResult,
                "EmailServerSettings",
                "HostAddress"),
            Timeout = ParseValue<int>(
                serverSettings.Values.Find(s => s.Key == "Timeout")?.Value,
                ref importResult,
                "EmailServerSettings",
                "Timeout"),
            RequireSsl = ParseValue<bool>(
                serverSettings.Values.Find(s => s.Key == "RequiresSSL")?.Value,
                ref importResult,
                "EmailServerSettings",
                "RequiresSSL"),
            Username = ParseValue<string>(
                serverSettings.Keys.Find(k => k.Key == "Credentials")?.Values.Find(s => s.Key == "UserName")?.Value,
                ref importResult,
                "EmailServerSettings",
                "UserName"),
            Password = ParseValue<string>(
                serverSettings.Keys.Find(k => k.Key == "Credentials")?.Values.Find(s => s.Key == "Password")?.Value,
                ref importResult,
                "EmailServerSettings",
                "Password"),
            UseIisPickupDirectory = ParseValue<bool>(
                serverSettings.Keys.Find(k => k.Key == "DeliveryMethod")?.Values.Find(s => s.Key == "UseIISPickupDirectory")?.Value,
                ref importResult,
                "EmailServerSettings",
                "UseIISPickupDirectory")
        };

        var messageConfiguration = new MessageConfiguration
        {
            Subject = ParseValue<string>(
                messageSettings.Keys.Find(k => k.Key == "Subject")?.Values.Find(s => s.Key == "Text")?.Value,
                ref importResult,
                "EmailMessageSettings",
                "Text"),
            BodyText = ParseValue<string>(
                messageSettings.Keys.Find(k => k.Key == "Body")?.Values.Find(s => s.Key == "Text")?.Value,
                ref importResult,
                "EmailMessageSettings",
                "HostAddress"),
            IsBodyHtml = ParseValue<bool>(
                messageSettings.Keys.Find(k => k.Key == "Body")?.Values.Find(s => s.Key == "IsHTML")?.Value,
                ref importResult,
                "EmailServerSettings",
                "HostAddress"),
            From = ParseValue<string>(
                messageSettings.Values.Find(s => s.Key == "From")?.Value,
                ref importResult,
                "EmailMessageSettings",
                "From"),
            To = ParseValue<string>(
                messageSettings.Values.Find(s => s.Key == "To")?.Value,
                ref importResult,
                "EmailMessageSettings",
                "To"),
            Cc = ParseValue<string>(
                messageSettings.Values.Find(s => s.Key == "CC")?.Value,
                ref importResult,
                "EmailMessageSettings",
                "CC"),
            Bcc = ParseValue<string>(
                messageSettings.Values.Find(s => s.Key == "BCC")?.Value,
                ref importResult,
                "EmailMessageSettings",
                "BCC"),
            ReplyTo = ParseValue<string>(
                messageSettings.Values.Find(s => s.Key == "ReplyTo")?.Value,
                ref importResult,
                "EmailMessageSettings",
                "ReplyTo")
        };

        return new EmailConfiguration()
        {
            EmailServerConfiguration = serverConfiguration,
            MessageConfiguration = messageConfiguration,
        };
    }

    internal static List<CustomSetting> GetCustomSettings(this ConfigurationFile config, ref ConfigurationImportResult importResult)
    {
        var result = new List<CustomSetting>();
        foreach (var customSettingKeys in config.CustomSettings.Keys.Where(k => k.Key != "Email"))
        {
            RecursivelyGetSettingValues(customSettingKeys, "CustomSettings", ref result);
        }
        foreach (var customSettingsKeyValue in config.CustomSettings.Values)
        {
            result.Add(new CustomSetting($"CustomSettings.{customSettingsKeyValue.Key}", AutoDetectDataType(customSettingsKeyValue.Value, out var type), type));
        }
        foreach (var customSetting in result)
        {
            importResult.AutoConversionResults.Add($"Custom setting \"{customSetting.Key}\" with value \"{customSetting.Value}\" was automatically converted to a {customSetting.DataType}.");
        }

        return result;
    }

    private static object? AutoDetectDataType(string value, out SettingDataType dataType)
    {
        if (bool.TryParse(value?.ToLower(), out var boolValue))
        {
            dataType = SettingDataType.Bool;
            return boolValue;
        }

        if (int.TryParse(value?.ToLower(), out var intValue))
        {
            dataType = SettingDataType.Int;
            return intValue;
        }

        if (Uri.TryCreate(value, UriKind.Absolute, out var uri))
        {
            dataType = SettingDataType.Uri;
            return uri;
        }

        dataType = SettingDataType.String;
        return value;
    }

    private static void RecursivelyGetSettingValues(CustomSettingsKeyValues customSettingKeys, string group, ref List<CustomSetting> result)
    {
        group = $"{group}.{customSettingKeys.Key}";
        result.AddRange(customSettingKeys.Values.Select(customSettingsKeyValue => new CustomSetting($"{group}.{customSettingsKeyValue.Key}", AutoDetectDataType(customSettingsKeyValue.Value, out var type), type)));

        foreach (var key in customSettingKeys.Keys)
        {
            RecursivelyGetSettingValues(key, group, ref result);
        }
    }

    private static TType? ParseValue<TType>(string? value, ref ConfigurationImportResult result, string section, string key)
    {
        if (value == null)
            return default;

        if (ValueConverters.ContainsKey(typeof(TType)))
        {
            try
            {
                return (TType)ValueConverters[typeof(TType)](value);
            }
            catch (Exception e)
            {
                result.RaiseValueConversionError(key, section, typeof(TType), value);
            }
        }
        else
        {
            result.RaiseNoConversionAvailableError(key, section, typeof(TType));
        }

        return default;
    }
}