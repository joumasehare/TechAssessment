using SettingsManager.Common.Settings;

namespace SettingsManager.Domain.Models.Settings;

public class Setting : IntIdentifiedEntity
{
    public required string LocalKey { get; set; }
    public string GlobalKey {
        get => $"{Group}.{LocalKey}";
        set { /*Required for EF */ }
    } 
    public required string Group { get; set; }
    public required SettingDataType SettingDataType { get; set; }
    public string? Value { get; set; }
}