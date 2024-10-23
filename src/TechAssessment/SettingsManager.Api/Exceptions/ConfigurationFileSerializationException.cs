namespace SettingsManager.Api.Exceptions;

public class ConfigurationFileSerializationException(string? message = ConfigurationFileSerializationException.DefaultMessage, Exception? innerException = null) : Exception(message, innerException)
{
    internal const string DefaultMessage = "Error during serialization of the configuration file";

    public ConfigurationFileSerializationException(string message) : this(message, null) { }

    public ConfigurationFileSerializationException(Exception innerException) : this(DefaultMessage, innerException) { }
}