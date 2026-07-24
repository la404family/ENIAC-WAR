using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace EniacWar;

public static class LocalizationManager
{
    private static Dictionary<string, Dictionary<string, string>> _translations = new();
    public static string CurrentLanguage { get; set; } = "EN";
    public static readonly string[] SupportedLanguages = { "EN", "FR", "ES", "DE", "IT", "PT-BR", "TR" };

    public static void Initialize(string directoryPath = "Content/Translations")
    {
        _translations.Clear();

        if (Directory.Exists(directoryPath))
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            foreach (var filePath in Directory.GetFiles(directoryPath, "*.json"))
            {
                try
                {
                    string json = File.ReadAllText(filePath);
                    var fileDict = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(json, options);
                    
                    if (fileDict != null)
                    {
                        foreach (var langKvp in fileDict)
                        {
                            string lang = langKvp.Key.ToUpper();
                            if (!_translations.ContainsKey(lang))
                            {
                                _translations[lang] = new Dictionary<string, string>();
                            }
                            
                            foreach (var stringKvp in langKvp.Value)
                            {
                                _translations[lang][stringKvp.Key] = stringKvp.Value;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading translation file {filePath}: " + ex.Message);
                }
            }
        }
    }

    public static string GetString(string key)
    {
        if (_translations != null && _translations.TryGetValue(CurrentLanguage, out var langDict))
        {
            if (langDict.TryGetValue(key, out var val))
            {
                return val;
            }
        }
        
        // Fallback to EN if key not found
        if (CurrentLanguage != "EN" && _translations != null && _translations.TryGetValue("EN", out var enDict))
        {
            if (enDict.TryGetValue(key, out var enVal))
            {
                return enVal;
            }
        }
        
        return $"[{key}]";
    }
}
