namespace SettingsManager.Domain.Models;

public interface IEntity<TIdentifier>
{
    public TIdentifier Id {get; set; }
}