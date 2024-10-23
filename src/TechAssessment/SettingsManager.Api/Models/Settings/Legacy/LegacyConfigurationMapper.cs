using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SettingsManager.Api.Utilities;
using System.Linq.Expressions;
using static System.Collections.Specialized.BitVector32;

namespace SettingsManager.Api.Models.Settings.Legacy
{
    internal static class LegacyConfigurationMapper
    {
        private static readonly Dictionary<Type, Func<string, object>> valueConverters = new()
        {
            {typeof(string), s => s},
            {typeof(int), s => Convert.ToInt32(s)},
            {typeof(bool), s => Convert.ToBoolean(s.ToLower())},
            {typeof(Uri), s => new Uri(s)},
        };

        internal static CompanySettings GetCompanySettings(this ConfigurationFile config, ref ConfigurationImportResult importResult)
        {
            CompanySettings result = new CompanySettings();
            result.CompanyName = ParseValue<string>(
                config.AppSettings.AppSettingsKeyValuePairs.Find(k => k.Key == "Company")?.Value, 
                ref importResult,
                "CompanySettings",
                "Company");

            result.License = ParseValue<string>(
                config.AppSettings.AppSettingsKeyValuePairs.Find(k => k.Key == "License")?.Value,
                ref importResult,
                "CompanySettings",
                "License");

            result.ServerAddress = ParseValue<Uri>(
                config.AppSettings.AppSettingsKeyValuePairs.Find(k => k.Key == "ServerAddress")?.Value,
                ref importResult,
                "CompanySettings",
                "ServerAddress");

            result.LoginAttemptLimit = ParseValue<int>(
                config.AppSettings.AppSettingsKeyValuePairs.Find(k => k.Key == "LoginAttemptLimit")?.Value,
                ref importResult,
                "CompanySettings",
                "LoginAttemptLimit");

            result.PasswordHistory = ParseValue<int>(
                config.AppSettings.AppSettingsKeyValuePairs.Find(k => k.Key == "PasswordHistory")?.Value,
                ref importResult,
                "CompanySettings",
                "PasswordHistory");

            return result;
        }

        private static TType? ParseValue<TType>(string? value, ref ConfigurationImportResult result, string section, string key)
        {
            if (value == null)
                return default;

            if (valueConverters.ContainsKey(typeof(TType)))
            {
                try
                {
                    return (TType)valueConverters[typeof(TType)](value);
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
}
