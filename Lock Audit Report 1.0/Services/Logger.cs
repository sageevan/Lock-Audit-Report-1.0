using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lock_Audit_Report_1._0.Services
{
    public static class Logger
    {
        private static readonly string LogFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");

        public static void Log(string message)
        {
            try
            {
                if (!Directory.Exists(LogFolder))
                    Directory.CreateDirectory(LogFolder);

                string file = Path.Combine(LogFolder, $"log_{DateTime.Now:yyyyMMdd}.txt");
                File.AppendAllText(file, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}{Environment.NewLine}");
            }
            catch { }
        }

        public static void LogException(Exception ex, string context = "")
        {
            Log($"ERROR in {context}: {ex.Message}\n{ex.StackTrace}");
        }
    }
}
