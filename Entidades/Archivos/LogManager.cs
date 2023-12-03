using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Archivos
{
    public static class LogManager
    {
        public static void LogException(Exception ex)
        {
            string logFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "20231207_Alumn", "logs.txt");

            string logMessage = $"[{DateTime.Now}] {ex.Message}\n{ex.StackTrace}\n\n";

            File.AppendAllText(logFilePath, logMessage);
        }
    }

}
