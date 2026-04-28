
using System.IO;

namespace PlantManagement.Comm.Logger;

public class LogService : ILogService
{
    private static readonly object _lock = new object();

    /// <summary>
    /// 로그
    /// </summary>
    public void LogMessage(string message)
    {
        try
        {
            var now = DateTime.Now;

            var logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SystemLog", now.ToString("yyyy"),
                now.ToString("MM"));
            
            // 디렉터리가 없으면 생성 (이미 있으면 아무런 동작 하지 않음)
            Directory.CreateDirectory(logDir);
            
            // 날짜별 파일 생성
            var logFile = Path.Combine(logDir, $"{now:yyyy-MM-dd}.txt");
            
            // 파일에 한 줄씩 Append
            lock (_lock)
            {
                using var fs = new FileStream(logFile, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                using var writer = new StreamWriter(fs);
                
                writer.WriteLine($"[{now:yyyy-MM-dd HH:mm:ss.fff}]\t{message}");
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex);
            Console.ResetColor();
        }
    }
}