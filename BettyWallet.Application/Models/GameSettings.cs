namespace BettyWallet.Application.Models
{
    public class GameSettings
    {
        public decimal MinBetAmount { get; set; }
        public decimal MaxBetAmount { get; set; }
        public WinProbabilities WinProbabilities { get; set; } = new();
    }
}
