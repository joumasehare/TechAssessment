namespace SettingsManager.Api.Models.Settings.Legacy;

public class ConfigurationImportResult
{
    public bool IsSuccessful => Errors.Count == 0 && FatalMessage == null;

    public List<ConfigurationImportValidationError> Errors { get; set; } = [];

    public List<string> AutoConversionResults { get; set; } = [];

    public string? FatalMessage { get; set; }

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
}

public class ConfigurationImportValidationError
{
    public string? Section { get; set; }
    public string? Field { get; set; }
    public string? Description { get; set; }
}