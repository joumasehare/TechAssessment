using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using AutoMapper.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SettingsManager.Api.Exceptions;
using SettingsManager.Api.Models.Settings;
using SettingsManager.Api.Models.Settings.Legacy;
using SettingsManager.Api.Utilities;
using SettingsManager.Common.Settings;
using SettingsManager.Data;
using SettingsManager.Domain.Models;
using SettingsManager.Domain.Models.Settings;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SettingsManager.Api.Settings
{
    public class SettingsApi(IRepository<Setting, int> settingRepository) : ISettingsApi
    {


        public bool GetSettingValue<TResult>(string key, out TResult value)
        {
            throw new NotImplementedException();
        }

        public bool GetSettingValue(string key, out string? value)
        {
            var setting = settingRepository.Get(s => s.GlobalKey == key).FirstOrDefault();
            value = setting?.Value;
            return setting != null;
        }

        public void SetSettingValue<TValue>(string key, SettingDataType settingDataType, TValue value)
        {
            GetKeyChain(key, out var localKey, out var domain);

            var setting = new Setting
            {
                Domain = domain,
                LocalKey = localKey,
                SettingDataType = settingDataType,
                Value = SettingsSerializationHelper.Serialize(settingDataType, value)
            };

            setting.Id = GetExistingSettingId(key);

            settingRepository.Save(setting);
        }

        public void SaveSettingEntity<TEntity>(TEntity entity)
        {
            var settingValues = new List<Setting>();
            RecursivelyGetSettingValues(typeof(TEntity), entity, ref settingValues);
            foreach (var settingValue in settingValues)
            {
                settingValue.Id = GetExistingSettingId(settingValue.GlobalKey);
                settingRepository.Save(settingValue);
            }
        }

        private static void RecursivelyGetSettingValues(Type type, object? instance, ref List<Setting> settingValues)
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
                        Domain = entityAttribute.Domain,
                        SettingDataType = propertyAttribute.DataType,
                        Value = SettingsSerializationHelper.Serialize(propertyAttribute.DataType, propertyInfo.GetValue(instance))
                    });
                }
                else
                {
                    if (propertyInfo.GetCustomAttribute(typeof(ChildSettingEntityAttribute)) is not null)
                    {
                        RecursivelyGetSettingValues(propertyInfo.PropertyType, propertyInfo.GetValue(instance), ref settingValues);
                    }
                }
            }
        }

        private int GetExistingSettingId(string key)
        {
            var existing = settingRepository.Get(s => s.GlobalKey == key).FirstOrDefault();
            if (existing != null)
            {
                return existing.Id;
            }

            if (settingRepository.Get(s => s.Domain == key).Any())
                throw new SettingKeyException($"The Setting Key \"{key}\" is already defined as a group. Keys must refer to a Setting key, not a Group key.");

            return 0;
        }

        private static List<string> GetKeyChain(string key, out string localKey, out string domain)
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
            domain = key.Remove(lastIndex, localKey.Length + 1);

            return result;
        }
    }
}
