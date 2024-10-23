using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SettingsManager.Common.Settings;

namespace SettingsManager.Api.Models.Settings
{
    public class Setting
    {
        public string LocalKey { get; set; }
        public string GlobalKey => $"{Group}.{LocalKey}";
        public string Group { get; set; }
        public SettingDataType SettingDataType { get; set; }
        public string? Value { get; set; }

        public Setting(Domain.Models.Settings.Setting? domain)
        {
            if (domain is null)
                return;

            LocalKey = domain.LocalKey;
            Group = domain.Group;
            SettingDataType = domain.SettingDataType;
            Value = domain.Value;
        }
    }
}
