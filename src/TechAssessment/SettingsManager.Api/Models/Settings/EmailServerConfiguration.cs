using SettingsManager.Api.Settings;
using SettingsManager.Common.Settings;

namespace SettingsManager.Api.Models.Settings;

[SettingEntity("Host.Email")]
public class EmailConfiguration
{
    [ChildSettingEntity]
    public EmailServerConfiguration? EmailServerConfiguration { get; set; }

    [ChildSettingEntity]
    public MessageConfiguration? MessageConfiguration { get; set; }
}

[SettingEntity("Host.Email.Message")]
public class MessageConfiguration
{
    [Setting(SettingDataType.String, nameof(Subject))]
    public string? Subject { get; set; }

    [Setting(SettingDataType.String, nameof(BodyText))]
    public string? BodyText { get; set; }

    [Setting(SettingDataType.Bool, nameof(IsBodyHtml))]
    public bool IsBodyHtml { get; set; }

    [Setting(SettingDataType.Email, nameof(From))]
    public string? From { get; set; }

    [Setting(SettingDataType.Email, nameof(To))]
    public string? To { get; set; }

    [Setting(SettingDataType.Email, nameof(Cc))]
    public string? Cc { get; set; }

    [Setting(SettingDataType.Email, nameof(Bcc))]
    public string? Bcc { get; set; }

    [Setting(SettingDataType.Email, nameof(ReplyTo))]
    public string? ReplyTo { get; set; }
}

[SettingEntity("Host.Email.Server")]
public class EmailServerConfiguration
{
    [Setting(SettingDataType.String, nameof(HostAddress))]
    public string? HostAddress { get; set; }

    [Setting(SettingDataType.Int, nameof(Timeout))]
    public int Timeout { get; set; }

    [Setting(SettingDataType.String, $"Credentials.{nameof(Username)}")]
    public string? Username { get; set; }

    [Setting(SettingDataType.String, $"Credentials.{nameof(Password)}")]
    public string? Password { get; set; }

    [Setting(SettingDataType.Bool, $"DeliveryMethod.{nameof(UseIisPickupDirectory)}")]
    public bool UseIisPickupDirectory { get; set; }

    [Setting(SettingDataType.Bool, nameof(RequireSsl))]
    public bool RequireSsl { get; set; }
}