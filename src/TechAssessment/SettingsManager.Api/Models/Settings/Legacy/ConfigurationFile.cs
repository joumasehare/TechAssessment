using SettingsManager.Api.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SettingsManager.Api.Models.Settings.Legacy
{
    [XmlRoot(ElementName = "configuration")]
    public class ConfigurationFile
    {
        [XmlElement("appSettings")] public AppSettings AppSettings { get; set; } = new();

        [XmlElement("customSettings")] public CustomSettingsKeyValues CustomSettings { get; set; } = new();
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
        [XmlElement("value")] public List<CustomSettingsKeyValue> Values { get; set; } = [];
        [XmlElement("key")] public List<CustomSettingsKeyValues> Keys { get; set; } = [];
    }
}
