namespace SettingsManager.Api.Models.Settings;

public class ProductClientSettings
{
    public int Id { get; set; }

    //This will be the key
    public string? WebsiteUrl { get; set; }
    public bool ShowHeader { get; set; }
    public bool ShowMenuBar { get; set; }
    public int SubMenusToShow { get; set; }
    public bool DisplayFullError { get; set; }
    public int Feature3ItemCount { get; set; }
}