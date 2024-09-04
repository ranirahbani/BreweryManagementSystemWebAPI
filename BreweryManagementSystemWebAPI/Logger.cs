using System;
using System.IO;

public static class Logger
{
    private static readonly string logFilePath = "log.txt";

    public static void LogError(string methodName, string errorMessage)
    {
        string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | {methodName} | ERROR | {errorMessage}";
        WriteLog(logEntry);
    }

    public static void LogInfo(string methodName, string message)
    {
        string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | {methodName} | INFO | {message}";
        WriteLog(logEntry);
    }

    private static void WriteLog(string logEntry)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine(logEntry);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to write to log file: {ex.Message}");
        }
    }
}
