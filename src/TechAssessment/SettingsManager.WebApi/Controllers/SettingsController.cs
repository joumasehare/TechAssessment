using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SettingsManager.Api.Exceptions;
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
    /// <summary>
    /// Uploads, parses, and saves settings from the legacy format.
    /// </summary>
    /// <param name="settingFile">The legacy settings file</param>
    /// <returns>A result of the import process</returns>
    [HttpPut("UploadLegacySettings")]
    [ProducesResponseType(200, Type = typeof(ConfigurationImportResult))]
    [ProducesResponseType(400, Type = typeof(ConfigurationImportResult))]
    public ActionResult<ConfigurationImportResult> Import(IFormFile settingFile)
    {
        try
        {
            var importResult = legacySettingImporter.ImportLegacyConfigurationFile(legacySettingImporter.ParseLegacyConfigurationFile(settingFile.OpenReadStream()));
            if (!importResult.IsSuccessful)
                return BadRequest(importResult);
            
            return Ok(importResult);
        }
        catch (Exception ex) when (ex is SettingsSerializationException or SettingKeyException)
        {
            return BadRequest(new ConfigurationImportResult(){ FatalMessage = ex.Message});
        }
    }

    /// <summary>
    /// Sets a setting from a string value.
    /// </summary>
    /// <param name="key">A key to uniquely identify a setting. A key must follow the prescribed format of Group.Group.Group.Key</param>
    /// <param name="type">The type of value being saved</param>
    /// <param name="value">The string representation of the value</param>
    /// <returns></returns>
    [ProducesResponseType(200, Type = typeof(string))]
    [ProducesResponseType(400, Type = typeof(string))]
    [HttpPost]
    public ActionResult SetSetting(string key, SettingDataType type, string value)
    {
        try
        {
            settingsApi.SerializeAndSetSettingValue(key, type, value);
            return Ok($"Setting {key} saved.");
        }
        catch (Exception ex) when (ex is SettingsSerializationException or SettingKeyException)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Gets a setting by key. It will always return the string serialized version of the value.
    /// </summary>
    /// <param name="key">A key to uniquely identify a setting. A key must follow the prescribed format of Group.Group.Group.Key</param>
    /// <returns></returns>
    [ProducesResponseType(200, Type = typeof(Setting))]
    [ProducesResponseType(404, Type = typeof(string))]
    [HttpGet("{key}")]
    public ActionResult<Setting> GetSetting(string key)
    {
        if (settingsApi.GetSettingValue(key, out var value))
            return Ok(value);
        else
            return NotFound();
    }

    /// <summary>
    /// Gets a structured representation of the HostSettings collection of settings.
    /// </summary>
    /// <returns></returns>
    [ProducesResponseType(200, Type = typeof(HostSettings))]
    [ProducesResponseType(404, Type = typeof(string))]
    [HttpGet("HostSettings")]
    public ActionResult<HostSettings> GetHostSettings()
    {
        if (settingsApi.GetSettingsEntity<HostSettings>(out var value))
            return Ok(value);
        else
            return NotFound();
    }

    /// <summary>
    /// Gets a structured representation of the ProductClientSettings collection of settings.
    /// </summary>
    /// <returns></returns>
    [ProducesResponseType(200, Type = typeof(ProductClientSettings))]
    [ProducesResponseType(404, Type = typeof(string))]
    [HttpGet("ClientSettings")]
    public ActionResult<ProductClientSettings> GetClientSettings()
    {
        if (settingsApi.GetSettingsEntity<ProductClientSettings>(out var value))
            return Ok(value);
        else
            return NotFound();
    }

    /// <summary>
    /// Gets a structured representation of the EmailConfiguration collection of settings.
    /// </summary>
    /// <returns></returns>
    [ProducesResponseType(200, Type = typeof(EmailConfiguration))]
    [ProducesResponseType(404, Type = typeof(string))]
    [HttpGet("EmailSettings")]
    public ActionResult<EmailConfiguration> GetEmailSettings()
    {
        if (settingsApi.GetSettingsEntity<EmailConfiguration>(out var value))
            return Ok(value);
        else
            return NotFound();
    }

    /// <summary>
    /// Gets a list of settings that are in a group and optionally of a group. By default, it will return child groups as well.
    /// </summary>
    /// <param name="group">The group key or portion thereof to use in the query</param>
    /// <param name="includeSubGroups">Decides if it must be in a specific group, or include subgroups</param>
    /// <returns></returns>
    [ProducesResponseType(200, Type = typeof(List<Setting>))]
    [HttpGet("SettingsInGroup")]
    public ActionResult<List<Setting>> GetSettingsInGroup(string group, bool includeSubGroups = true)
    {
        return Ok(settingsApi.GetSettingsInGroup(group, includeSubGroups));
    }
}