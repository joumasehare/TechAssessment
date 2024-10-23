using SettingsManager.Api.Settings;
using SettingsManager.Common.Settings;

namespace SettingsManager.Api.Models.Settings;

[SettingEntity("Client.Web")]
public class ProductClientSettings
{
    [Setting(SettingDataType.Uri, nameof(WebsiteUrl))]
    public Uri? WebsiteUrl { get; set; }

    [Setting(SettingDataType.Bool, nameof(ShowHeader))]
    public bool ShowHeader { get; set; }

    [Setting(SettingDataType.Bool, nameof(ShowMenuBar))]
    public bool ShowMenuBar { get; set; }

    [Setting(SettingDataType.Int, nameof(SubMenusToShow))]
    public int SubMenusToShow { get; set; }

    [Setting(SettingDataType.Bool, nameof(DisplayFullError))]
    public bool DisplayFullError { get; set; }
}