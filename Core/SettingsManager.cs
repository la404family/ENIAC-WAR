using System;
using System.IO;
using System.Text.Json;

namespace EniacWar;

public class GameSettings
{
    public string Language { get; set; } = "EN";
    public int ResolutionWidth { get; set; } = 1280;
    public int ResolutionHeight { get; set; } = 720;
    public bool IsFullScreen { get; set; } = false;
}

public static class SettingsManager
{
    public static GameSettings Settings { get; private set; } = new GameSettings();
    private static string _filePath;

    public static void Initialize()
    {
        _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.json");
        Load();
    }

    public static void Load()
    {
        if (File.Exists(_filePath))
        {
            try
            {
                string json = File.ReadAllText(_filePath);
                var loaded = JsonSerializer.Deserialize<GameSettings>(json);
                if (loaded != null)
                {
                    Settings = loaded;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading settings: " + ex.Message);
            }
        }
    }

    public static void Save()
    {
        try
        {
            string json = JsonSerializer.Serialize(Settings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error saving settings: " + ex.Message);
        }
    }
}
