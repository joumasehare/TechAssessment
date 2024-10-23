using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SettingsManager.Common.Settings;

namespace SettingsManager.Domain.Models.Settings
{
    public class Setting : IntIdentifiedEntity
    {
        public required string LocalKey { get; set; }
        public string GlobalKey {
            get => $"{Domain}.{LocalKey}";
            set { }
        } 
        public required string Domain { get; set; }
        public required SettingDataType SettingDataType { get; set; }
        public string? Value { get; set; }
    }
}
