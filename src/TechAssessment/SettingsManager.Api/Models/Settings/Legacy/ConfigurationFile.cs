using System.Xml.Serialization;

namespace AcmeProduct.Api.Models.Settings.Legacy;

[XmlRoot(ElementName = "configuration")]
public class ConfigurationFile
{
    [XmlElement("appSettings")] public AppSettings AppSettings { get; set; } = new();

    [XmlElement("customSettings")] public CustomSettingsKeyValues? CustomSettings { get; set; }
}

public class AppSettings
{
    [XmlElement("add")] public List<AppSettingsKeyValuePair> AppSettingsKeyValuePairs { get; set; } = [];
}

public class AppSettingsKeyValuePair
{
    [XmlAttribute("key")] public required string Key { get; set; }
    [XmlAttribute("value")] public string? Value { get; set; }
}

public class CustomSettingsKeyValue
{
    [XmlAttribute("name")] public required string Key { get; set; }

    [XmlText] public required string Value { get; set; }
}

public class CustomSettingsKeyValues
{
    [XmlAttribute("name")] public required string Key { get; set; }
    [XmlElement("value")] public List<CustomSettingsKeyValue> Values { get; set; } = [];
    [XmlElement("key")] public List<CustomSettingsKeyValues> Keys { get; set; } = [];
}