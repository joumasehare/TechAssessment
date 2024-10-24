﻿using System.Text;
using Microsoft.AspNetCore.Http;

namespace SettingsManager.Web.Common.Extentions;

public static class FormFileExtensions
{
    public static async Task<string> ReadAsStringAsync(this IFormFile file)
    {
        var result = new StringBuilder();
        using (var reader = new StreamReader(file.OpenReadStream()))
        {
            while (reader.Peek() >= 0)
                result.AppendLine(await reader.ReadLineAsync());
        }
        return result.ToString();
    }
}