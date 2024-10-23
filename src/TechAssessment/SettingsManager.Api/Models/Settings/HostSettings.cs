using AcmeProduct.Api.Settings;

namespace AcmeProduct.Api.Models.Settings;

[SettingEntity("Host")]
public class HostSettings
{
    [ChildSettingEntity]
    public CompanySettings? CompanySettings { get; set; }

    [ChildSettingEntity]
    public EmailConfiguration? EmailConfiguration { get; set; }
}