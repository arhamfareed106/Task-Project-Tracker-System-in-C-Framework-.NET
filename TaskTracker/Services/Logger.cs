using TaskTracker.Services.Interfaces;
using System.IO;

namespace TaskTracker.Services
{
    /// <summary>
    /// Singleton logger service for logging application events
    /// </summary>
    public class Logger : ILogger
    {
        private static Logger? _instance;
        private static readonly object _lock = new object();
        private readonly string _logFilePath;

        /// <summary>
        /// Private constructor to prevent instantiation
        /// </summary>
        private Logger()
        {
            _logFilePath = Path.Combine("DataFiles", "activity_log.txt");
            
            // Ensure the DataFiles directory exists
            var directory = Path.GetDirectoryName(_logFilePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        /// <summary>
        /// Gets the singleton instance of the Logger
        /// </summary>
        public static Logger Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        _instance ??= new Logger();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Logs an informational message
        /// </summary>
        /// <param name="message">The message to log</param>
        public void LogInfo(string message)
        {
            Log("INFO", message);
        }

        /// <summary>
        /// Logs a warning message
        /// </summary>
        /// <param name="message">The warning message to log</param>
        public void LogWarning(string message)
        {
            Log("WARNING", message);
        }

        /// <summary>
        /// Logs an error message
        /// </summary>
        /// <param name="message">The error message to log</param>
        public void LogError(string message)
        {
            Log("ERROR", message);
        }

        /// <summary>
        /// Writes a log entry to the log file
        /// </summary>
        /// <param name="level">The log level</param>
        /// <param name="message">The message to log</param>
        private void Log(string level, string message)
        {
            try
            {
                var logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}{Environment.NewLine}";
                File.AppendAllText(_logFilePath, logEntry);
            }
            catch (Exception ex)
            {
                // If we can't log to file, output to console as fallback
                Console.WriteLine($"Failed to write to log file: {ex.Message}");
                Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}");
            }
        }
    }
}