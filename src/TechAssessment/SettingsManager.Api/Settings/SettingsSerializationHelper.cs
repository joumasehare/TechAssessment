using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using SettingsManager.Common.Settings;

namespace SettingsManager.Api.Settings
{
    public static class SettingsSerializationHelper
    {
        private static readonly Dictionary<SettingDataType, Func<object, string>> Serializers = new()
        {
            {SettingDataType.String, s => (string) s},
            {SettingDataType.Int, s => ((int) s).ToString()},
            {SettingDataType.Bool, s => ((bool) s).ToString()},
            {SettingDataType.Uri, s => ((Uri) s).ToString()},
            {SettingDataType.Email, s =>
            {
                var emailList = (string)s;
                //Validate

                //TODO: Validate
                return emailList;
            }},
        };

        public static string? Serialize(SettingDataType settingDataType, object? value)
        {
            return value is null ? null : Serializers[settingDataType](value);
        }
    }
}
