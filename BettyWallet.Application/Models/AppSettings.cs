namespace BettyWallet.Application.Models
{
    /// <summary>
    /// Main configuration class that loads settings from appsettings.json
    /// </summary>
    public class AppSettings
    {
        public WalletSettings WalletSettings { get; set; } = new();
        public GameSettings GameSettings { get; set; } = new();
        public LoggingSettings Logging { get; set; } = new();
    }
}
