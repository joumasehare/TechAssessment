namespace SettingsManager.Api.Models.Settings;

public class FeatureToggle
{
    public int Id { get; set; }

    //This will be the key
    public required string FeatureName { get; set; }
    public bool IsEnabled { get; set; }
}