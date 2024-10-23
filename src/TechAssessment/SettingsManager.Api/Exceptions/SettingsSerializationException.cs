namespace AcmeProduct.Api.Exceptions;

public class SettingsSerializationException : Exception
{
    public SettingsSerializationException(string message) : base(message) { }
    public SettingsSerializationException(string message, Exception innerException) : base(message, innerException) { }
}