namespace BettyWallet.Application.Models
{
    public class LoggingSettings
    {
        public string Default { get; set; } = "Information";
        public bool LogToFile { get; set; }
        public string LogFilePath { get; set; } = "logs/game_log.txt";
    }
}
