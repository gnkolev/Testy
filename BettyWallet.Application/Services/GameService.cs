using BettyWallet.Application.Interfaces;
using BettyWallet.Domain.Entities;
using BettyWallet.Domain.Enums;

namespace BettyWallet.Application.Services
{
    /// <summary>
    /// Handles game logic, including betting and win calculations.
    /// </summary>
    public class GameService : IGameService
    {
        private readonly ILoggerService _loggerService;
        private readonly Random _random;

        public GameService(ILoggerService loggerService, Random? random = null)
        {
            _loggerService = loggerService ?? throw new ArgumentNullException(nameof(loggerService));
            _random = random ?? new Random();
        }

        public GameResult Play(Player player, decimal betAmount)
        {
            ValidateBet(player, betAmount);

            if (!player.Wallet.Withdraw(betAmount))
            {
                PrintMessage("Not enough balance to place this bet.", ConsoleColor.Red);
                return new GameResult(BetResult.Lost, 0);
            }

            var result = DetermineGameOutcome(betAmount);
            if (result.WinAmount > 0)
            {
                player.Wallet.Deposit(result.WinAmount);
            }

            _loggerService.LogGameResult(result, betAmount);

            PrintGameResult(result, player.Wallet.Balance);

            return result;
        }

        private void ValidateBet(Player player, decimal betAmount)
        {
            if (player == null) throw new ArgumentNullException(nameof(player), "Player cannot be null.");
            if (betAmount < 1 || betAmount > 10) throw new ArgumentException("Bet amount must be between $1 and $10.");
        }

        private GameResult DetermineGameOutcome(decimal betAmount)
        {
            int chance = _random.Next(1, 101); // 1 to 100
            BetResult result;
            decimal winAmount = 0;

            if (chance <= 50)
            {
                result = BetResult.Lost;
            }
            else if (chance <= 90)
            {
                result = BetResult.WinX2;
                winAmount = betAmount * 2;
            }
            else
            {
                result = BetResult.WinX10;
                winAmount = betAmount * 10;
            }

            return new GameResult(result, winAmount);
        }

        private void PrintGameResult(GameResult result, decimal balance)
        {
            ConsoleColor color = (result.Result == BetResult.Lost) ? ConsoleColor.Red : ConsoleColor.Green;
            PrintMessage($"Game Result: {result.Result}. Won: ${result.WinAmount}. New Balance: ${balance}", color);
        }

        private void PrintMessage(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}