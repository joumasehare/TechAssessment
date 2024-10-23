namespace SettingsManager.Api.Settings;

public class SettingEntityAttribute(string domain) : Attribute
{
    public string Domain { get; } = domain;
}