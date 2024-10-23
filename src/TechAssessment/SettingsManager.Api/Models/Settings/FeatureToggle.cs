using AcmeProduct.Common.Settings;

namespace AcmeProduct.Api.Models.Settings;

public class FeatureToggle
{
    public required string FeatureName { get; set; }
    public bool IsEnabled { get; set; }
    public Dictionary<string, (SettingDataType DataType, object Value)> AdditionalSettings { get; set; } = new();
}