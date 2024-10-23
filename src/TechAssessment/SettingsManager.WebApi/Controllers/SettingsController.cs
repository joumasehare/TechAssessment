using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SettingsManager.Api.Models.Settings.Legacy;
using SettingsManager.Api.Settings;
using SettingsManager.Api.Settings.Legacy;
using SettingsManager.Common.Settings;
using SettingsManager.Web.Common.Extentions;

namespace SettingsManager.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly ILegacySettingImporter legacySettingImporter;
        private readonly ISettingsApi settingsApi;

        public SettingsController(ILegacySettingImporter legacySettingImporter, ISettingsApi settingsApi)
        {
            this.legacySettingImporter = legacySettingImporter;
            this.settingsApi = settingsApi;
        }

        [HttpPut("UploadLegacySettings")]
        public async Task<ConfigurationImportResult> Import(IFormFile settingFile)
        {
            var result = legacySettingImporter.ParseLegacyConfigurationFile(settingFile.OpenReadStream());
            return legacySettingImporter.ImportLegacyConfigurationFile(result);
        }

        [HttpPost()]
        public void SaveSetting(string key, SettingDataType type, string value)
        {
            settingsApi.SetSettingValue(key, type, value);
        }

        [HttpGet("{key}")]
        public ActionResult<string> GetSetting(string key)
        {
            if (settingsApi.GetSettingValue(key, out var value))
                return Ok(value);
            else
                return NotFound();
        }
    }
}
