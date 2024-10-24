﻿using AcmeProduct.Api.Models.Settings;
using AcmeProduct.Common.Settings;

namespace AcmeProduct.Api.Settings;

public interface ISettingsApi
{
    bool GetSettingValue<TResult>(string key, out TResult? value);
    bool GetSettingValue(string key, out Setting? value);
    bool GetSettingsEntity<TEntity>(out TEntity? value) where TEntity : class, new();
    List<Setting> GetSettingsInGroup(string group, bool includeSubGroups = true);
    void SetSettingValue<TValue>(string key, SettingDataType settingDataType, TValue value);
    void SerializeAndSetSettingValue(string key, SettingDataType settingDataType, string? value);
    void SetSettingEntity<TEntity>(TEntity companySettings);
}