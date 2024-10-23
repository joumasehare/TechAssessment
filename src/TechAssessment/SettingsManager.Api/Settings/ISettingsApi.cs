using SettingsManager.Api.Models.Settings;
using SettingsManager.Common.Settings;

namespace SettingsManager.Api.Settings;

public interface ISettingsApi
{
    bool GetSettingValue<TResult>(string key, out TResult value);
    bool GetSettingValue(string key, out string? value);
    void SetSettingValue<TValue>(string key, SettingDataType settingDataType, TValue value);
    void SaveSettingEntity<TEntity>(TEntity companySettings);
}