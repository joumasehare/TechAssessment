using AcmeProduct.Api.Settings;
using AcmeProduct.Common.Settings;

namespace AcmeProduct.Api.Models.Settings;

[SettingEntity("Host.Company")]
public class CompanySettings
{
    [Setting(SettingDataType.String, nameof(CompanyName))]
    public string? CompanyName { get; set; }

    [Setting(SettingDataType.String, nameof(License))]
    public string? License { get; set; }

    [Setting(SettingDataType.Int, nameof(LoginAttemptLimit))]
    public int LoginAttemptLimit { get; set; }

    [Setting(SettingDataType.Int, nameof(PasswordHistory))]
    public int PasswordHistory { get; set; }

    [Setting(SettingDataType.Uri, nameof(ServerAddress))]
    public Uri? ServerAddress { get; set; }
}