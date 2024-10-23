using AcmeProduct.Common.Settings;

namespace AcmeProduct.Api.Models.Settings;

internal class CustomSetting(string key, object? value, SettingDataType dataType)
{
    public string Key { get; } = key;
    public object? Value { get; } = value;
    public SettingDataType DataType { get; set; } = dataType;
}