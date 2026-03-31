namespace MessageExchanger.Server.Services
{
    public class Logger
    {
        // Will log messages to where the app is with the name as the date
        private readonly string _logFilePath = $"{AppDomain.CurrentDomain.BaseDirectory}/{DateTime.Now:yyyy-MM-dd}.log.txt";

        public Logger()
        {
            // Initialize the log file with a title saying the server has started
            Log("Server started.");
        }

        public void Log(string message)
        {
            try
            {
                // Ensure the directory exists
                Directory.CreateDirectory(Path.GetDirectoryName(_logFilePath)!);

                // Append the message to the log file with a timestamp
                File.AppendAllText(_logFilePath, $"{DateTime.Now:HH:mm:ss} -> {message}{Environment.NewLine}");
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during logging (e.g., file access issues)
                Console.WriteLine($"Logging failed: {ex.Message}");
            }
        }
    }
}
