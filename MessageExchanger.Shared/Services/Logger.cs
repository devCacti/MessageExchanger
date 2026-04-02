namespace MessageExchanger.Server.Services
{
    public class Logger
    {
        // Will log messages to where the app is with the name as the date
        private static readonly string _logFilePath = $"{AppDomain.CurrentDomain.BaseDirectory}/{DateTime.Now:yyyy-MM-dd}.log.txt";
        private static readonly object _lock = new object();

        public Logger()
        {
            // Initialize the log file with a title saying the server has started
            Log("Server started.");
        }

        public static void Log(string message)
        {
            string logEntry = $"[{DateTime.Now:HH:mm:ss}] {message}";

            // Print to Console so you see it live
            Console.WriteLine(logEntry);

            // Write to File (lock ensures thread-safety if multiple clients log at once)
            lock (_lock)
            {
                try
                {
                    File.AppendAllText(_logFilePath, logEntry + Environment.NewLine);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Critical Error writing to log file: {ex.Message}");
                }
            }
        }
    }
}
