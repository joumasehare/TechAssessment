using Microsoft.AspNetCore.Mvc;
using SettingsManager.Api.Models.Settings;
using SettingsManager.Api.Models.Settings.Legacy;
using SettingsManager.Api.Settings;
using SettingsManager.Api.Settings.Legacy;
using SettingsManager.Common.Settings;

namespace SettingsManager.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SettingsController(ILegacySettingImporter legacySettingImporter, ISettingsApi settingsApi)
    : ControllerBase
{
    [HttpPut("UploadLegacySettings")]
    public ActionResult<ConfigurationImportResult> Import(IFormFile settingFile)
    {
        var result = legacySettingImporter.ParseLegacyConfigurationFile(settingFile.OpenReadStream());
        return Ok(legacySettingImporter.ImportLegacyConfigurationFile(result));
    }

    [HttpPost]
    public void SaveSetting(string key, SettingDataType type, string value)
    {
        settingsApi.SetSettingValue(key, type, value);
        Ok();
    }

    [HttpGet("{key}")]
    public ActionResult<Setting> GetSetting(string key)
    {
        if (settingsApi.GetSettingValue(key, out var value))
            return Ok(value);
        else
            return NotFound();
    }

    [HttpGet("HostSettings")]
    public ActionResult<HostSettings> GetHostSettings()
    {
        if (settingsApi.GetSettingsEntity<HostSettings>(out var value))
            return Ok(value);
        else
            return NotFound();
    }

    [HttpGet("EmailSettings")]
    public ActionResult<EmailConfiguration> GetEmailSettings()
    {
        if (settingsApi.GetSettingsEntity<EmailConfiguration>(out var value))
            return Ok(value);
        else
            return NotFound();
    }

    [HttpGet("SettingsInGroup")]
    public ActionResult<List<Setting>> GetSettingsInGroup(string group, bool includeSubGroups = true)
    {
        return Ok(settingsApi.GetSettingsInGroup(group, includeSubGroups));
    }
}