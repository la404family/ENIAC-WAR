using System;
using System.IO;

try
{
    using var game = new EniacWar.Game1();
    game.Run();
}
catch (Exception ex)
{
    string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "crash.log");
    string logContent = $"[{DateTime.Now}] FATAL ERROR:\n{ex.ToString()}\n\n";
    File.AppendAllText(logPath, logContent);
    throw;
}
