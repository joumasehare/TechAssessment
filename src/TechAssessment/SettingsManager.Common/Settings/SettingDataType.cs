using System.Text.Json.Serialization;

namespace SettingsManager.Common.Settings;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SettingDataType : byte
{
    String = 1,
    Email = 2,
    Uri = 3,
    Int = 4,
    Bool = 5
}