using System.Reflection;
using System.Text.RegularExpressions;
using AutoMapper.Internal;
using SettingsManager.Api.Exceptions;
using SettingsManager.Common.Settings;
using SettingsManager.Data;
using SettingsManager.Domain.Models.Settings;

namespace SettingsManager.Api.Settings;

public class SettingsApi(IRepository<Setting, int> settingRepository) : ISettingsApi
{
    public bool GetSettingValue<TResult>(string key, out TResult? value)
    {
        var setting = settingRepository.Get(s => s.GlobalKey == key).FirstOrDefault();

        if (setting is null)
        {
            value = default;
            return false;
        }

        value = SettingsSerializationHelper.Deserialize<TResult>(setting.Value);
        return true;
    }

    public bool GetSettingValue(string key, out Models.Settings.Setting? value)
    {
        value = new Models.Settings.Setting(settingRepository.Get(s => s.GlobalKey == key).FirstOrDefault());
        return !string.IsNullOrEmpty(value.GlobalKey);
    }

    public bool GetSettingsEntity<TEntity>(out TEntity? value) where TEntity : class, new()
    {
        value = (TEntity?)RecursivelyGetSettingValues(typeof(TEntity));
        return value != null;
    }

    public List<Models.Settings.Setting> GetSettingsInGroup(string group, bool includeSubGroups = true)
    {
        if (includeSubGroups)
            return settingRepository.Get(s => s.Group.StartsWith(group)).Select(i => new Models.Settings.Setting(i)).ToList();
        else
            return settingRepository.Get(s => s.Group == group).Select(i => new Models.Settings.Setting(i)).ToList();
    }

    public void SetSettingValue<TValue>(string key, SettingDataType settingDataType, TValue value)
    {
        GetKeyChain(key, out var localKey, out var group);

        var setting = new Setting
        {
            Group = group,
            LocalKey = localKey,
            SettingDataType = settingDataType,
            Value = SettingsSerializationHelper.Serialize(settingDataType, value),
            Id = GetExistingSettingId(key)
        };

        settingRepository.Save(setting);
    }

    public void SetSettingEntity<TEntity>(TEntity entity)
    {
        var settingValues = new List<Setting>();
        RecursivelySetSettingValues(typeof(TEntity), entity, ref settingValues);
        foreach (var settingValue in settingValues)
        {
            settingValue.Id = GetExistingSettingId(settingValue.GlobalKey);
            settingRepository.Save(settingValue);
        }
    }

    private object? RecursivelyGetSettingValues(Type objectType)
    {
        var entityAttribute = (SettingEntityAttribute?)Attribute.GetCustomAttribute(objectType, typeof(SettingEntityAttribute));
        if (entityAttribute is null)
            return default;

        var instance = Activator.CreateInstance(objectType);
        foreach (var propertyInfo in objectType.GetProperties())
        {
            if (!propertyInfo.IsPublic() || !propertyInfo.CanBeSet()) continue;

            var propertyAttribute = (SettingAttribute?)propertyInfo.GetCustomAttribute(typeof(SettingAttribute));
            if (propertyAttribute is not null)
            {
                var setting = settingRepository
                    .Get(s => s.GlobalKey == $"{entityAttribute.Group}.{propertyAttribute.PartialKey}")
                    .FirstOrDefault();

                if (setting is not null)
                {
                    propertyInfo.SetValue(instance, SettingsSerializationHelper.Deserialize(propertyInfo.PropertyType, setting.Value));
                }
            }
            else
            {
                if (propertyInfo.GetCustomAttribute(typeof(ChildSettingEntityAttribute)) is not null)
                {
                    propertyInfo.SetValue(instance, RecursivelyGetSettingValues(propertyInfo.PropertyType));
                }
            }
        }

        return instance;
    }

    private static void RecursivelySetSettingValues(Type type, object? instance, ref List<Setting> settingValues)
    {
        if (instance is null)
            return;

        var entityAttribute = (SettingEntityAttribute?)Attribute.GetCustomAttribute(type, typeof(SettingEntityAttribute));
        if (entityAttribute is null)
            return;

        foreach (var propertyInfo in type.GetProperties())
        {
            if (!propertyInfo.IsPublic() || !propertyInfo.CanRead) continue;

            var propertyAttribute = (SettingAttribute?)propertyInfo.GetCustomAttribute(typeof(SettingAttribute));
            if (propertyAttribute is not null)
            {
                settingValues.Add(new Setting()
                {
                    LocalKey = propertyAttribute.PartialKey,
                    Group = entityAttribute.Group,
                    SettingDataType = propertyAttribute.DataType,
                    Value = SettingsSerializationHelper.Serialize(propertyAttribute.DataType, propertyInfo.GetValue(instance))
                });
            }
            else
            {
                if (propertyInfo.GetCustomAttribute(typeof(ChildSettingEntityAttribute)) is not null)
                {
                    RecursivelySetSettingValues(propertyInfo.PropertyType, propertyInfo.GetValue(instance), ref settingValues);
                }
            }
        }
    }

    private int GetExistingSettingId(string key)
    {
        var existing = settingRepository.Get(s => s.GlobalKey == key).FirstOrDefault();
        if (existing != null)
        {
            settingRepository.DetachEntity(existing);
            return existing.Id;
        }

        if (settingRepository.Get(s => s.Group == key).Any())
            throw new SettingKeyException($"The Setting Key \"{key}\" is already defined as a group. Keys must refer to a Setting key, not a Group key.");

        return 0;
    }

    private static List<string> GetKeyChain(string key, out string localKey, out string group)
    {
        var lastIndex = key.LastIndexOf('.');
        if (lastIndex == -1 || Regex.IsMatch(key, "(\\.{2,})+( )+(\\.$)+", RegexOptions.Singleline))
            throw new SettingKeyException($"The Setting Key \"{key}\" does not meet the requirements to be a key. " +
                                          $"{Environment.NewLine}Keys must refer to a Setting key, not a Group key." +
                                          $"{Environment.NewLine}Keys must also not contain whitespace such as \"Group.My SubGroup.SettingKey\"" +
                                          $"{Environment.NewLine}Keys must not contain blank groups such as \"Group.MySubGroup..SettingKey\"" +
                                          $"{Environment.NewLine}Keys must specify a Setting Key, and not a blank key such as \"Group.MySubGroup.\"");

        var result = new List<string>();

        string? previousPart = null;
        var keyParts = key.Split('.');
        for (int k = 0; k < keyParts.Length - 1; k++)
        {
            if (previousPart == null)
                previousPart = keyParts[k];
            else
                previousPart += $".{keyParts[k]}";

            result.Add(previousPart);
        }

        localKey = keyParts[^1];
        group = key.Remove(lastIndex, localKey.Length + 1);

        return result;
    }
}