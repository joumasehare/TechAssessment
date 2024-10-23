using SettingsManager.Api.Exceptions;
using SettingsManager.Common.Settings;

namespace SettingsManager.Api.Settings;

public static class SettingsSerializationHelper
{
    private static readonly Dictionary<SettingDataType, Func<object, string>> Serializers = new()
    {
        {SettingDataType.String, s => (string) s},
        {SettingDataType.Int, s => ((int) s).ToString()},
        {SettingDataType.Bool, s => ((bool) s).ToString()},
        {SettingDataType.Uri, s => ((Uri) s).ToString()},
        {SettingDataType.Email, s =>
        {
            var emailList = (string)s;
            //Validate

            //TODO: Validate
            return emailList;
        }},
    };

    private static readonly Dictionary<Type, Func<string, object?>> Deserializers = new()
    {
        {typeof(string), s => s},
        {typeof(int), s => Convert.ToInt32(s)},
        {typeof(bool), s => Convert.ToBoolean(s)},
        {typeof(Uri), s => new Uri(s)}
    };

    public static string? Serialize(SettingDataType settingDataType, object? value)
    {
        return value is null ? null : Serializers[settingDataType](value);
    }

    public static TResult? Deserialize<TResult>(string? settingValue)
    {
        return (TResult?)Deserialize(typeof(TResult), settingValue);
    }

    public static object? Deserialize(Type propertyType, string? settingValue)
    {
        if (!Deserializers.TryGetValue(propertyType, out var deserializer))
            throw new SettingsSerializationException($"There's no deserializer for type {propertyType}");

        return settingValue is null ? null : deserializer(settingValue);
    }
}