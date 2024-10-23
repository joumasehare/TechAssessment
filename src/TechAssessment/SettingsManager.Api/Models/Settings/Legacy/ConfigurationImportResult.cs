namespace SettingsManager.Api.Models.Settings.Legacy;

public class ConfigurationImportResult
{
    public bool IsSuccessful => Errors.Count == 0;
    public List<ConfigurationImportValidationError> Errors { get; set; } = [];
    public List<string> AutoConversionResults { get; set; } = [];

    public void RaiseRequiredError(string key, string section)
    {
        Errors.Add(new ConfigurationImportValidationError()
        {
            Field = key,
            Description = "The key is required",
            Section = section
        });
    }

    public void RaiseNoConversionAvailableError(string key, string section, Type concretePropertyPropertyType)
    {
        Errors.Add(new ConfigurationImportValidationError()
        {
            Field = key,
            Description = $"There does not exist a conversion to property type {concretePropertyPropertyType}",
            Section = section
        });
    }

    public void RaiseValueConversionError(string key, string section, Type concretePropertyPropertyType, string value)
    {
        Errors.Add(new ConfigurationImportValidationError()
        {
            Field = key,
            Description = $"A conversion error for value \"{value}\" to type {concretePropertyPropertyType.FullName} occured",
            Section = section
        });
    }

    public void RaiseAddFeatureToggleError(string toggleName)
    {
        Errors.Add(new ConfigurationImportValidationError()
        {
            Field = toggleName,
            Description = $"An error occured trying to convert feature toggle \"{toggleName}\". Please ensure that the config value is a boolean.",
            Section = "FeatureToggles"
        });
    }
}

public class ConfigurationImportValidationError
{
    public string Section { get; set; }
    public string Field { get; set; }
    public string Description { get; set; }
}