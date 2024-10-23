using SettingsManager.Common.Settings;

namespace SettingsManager.Api.Settings;

public class SettingEntityAttribute(string group) : Attribute
{
    public string Group { get; } = group;
}

public class SettingAttribute(SettingDataType dataType, string partialKey) : Attribute
{
    public SettingDataType DataType { get; } = dataType;
    public string PartialKey { get; } = partialKey;
}

public class ChildSettingEntityAttribute : Attribute
{
}