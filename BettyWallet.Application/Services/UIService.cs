using BettyWallet.Domain.Entities;
using BettyWallet.Application.Interfaces;

namespace BettyWallet.Application.Services
{
    public class UIService : IUIService
    {
        private readonly IWalletService _walletService;
        private readonly IGameService _gameService;
        private readonly ITransactionHistoryService _transactionHistoryService;
        private readonly Player _player;

        public UIService(IWalletService walletService, IGameService gameService, ITransactionHistoryService transactionHistoryService, Models.AppSettings appSettings)
        {
            _walletService = walletService;
            _gameService = gameService;
            _transactionHistoryService = transactionHistoryService;
            _player = new Player();
        }

        public void Run()
        {
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Welcome to Betty Wallet System!");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine($"Your starting balance is: ${_player.Wallet.Balance}");
            Console.ResetColor();

            while (true)
            {
                ShowMenu();
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": HandleDeposit(); break;
                    case "2": HandleWithdrawal(); break;
                    case "3": HandleBet(); break;
                    case "4": ShowTransactionHistory(); break;
                    case "5":
                        Console.WriteLine("Thanks for playing! Goodbye!");
                        return;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid choice. Please try again.");
                        Console.ResetColor();
                        break;
                }

                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine($"Current Balance: ${_player.Wallet.Balance}");
                Console.ResetColor();
            }
        }

        private void ShowMenu()
        {
            Console.WriteLine("\nChoose an action:");
            Console.WriteLine("1. Deposit Money");
            Console.WriteLine("2. Withdraw Money");
            Console.WriteLine("3. Place a Bet");
            Console.WriteLine("4. View Transaction History");
            Console.WriteLine("5. Exit");
            Console.Write("Enter your choice: ");
        }

        private void HandleDeposit()
        {
            decimal amount = GetValidAmount("Enter deposit amount: ");
            _walletService.Deposit(_player.Wallet, amount);
        }

        private void HandleWithdrawal()
        {
            decimal amount = GetValidAmount("Enter withdrawal amount: ");
            _walletService.Withdraw(_player.Wallet, amount);
        }

        private void HandleBet()
        {
            decimal betAmount = GetValidAmount("Enter bet amount ($1 - $10): ", 1, 10);
            _gameService.Play(_player, betAmount);
        }

        private void ShowTransactionHistory()
        {
            Console.WriteLine("Transaction History:");
            var transactions = _transactionHistoryService.LoadTransactions();

            if (transactions.Count == 0)
            {
                Console.WriteLine("No transactions found.");
            }
            else
            {
                foreach (var transaction in transactions)
                {
                    Console.WriteLine($"{transaction.Timestamp} | {transaction.Type} | ${transaction.Amount:F2}");
                }
            }
        }

        private decimal GetValidAmount(string message, decimal min = 0.01m, decimal max = decimal.MaxValue)
        {
            decimal amount;
            while (true)
            {
                Console.Write(message);
                if (decimal.TryParse(Console.ReadLine(), out amount) && amount >= min && amount <= max)
                {
                    return amount;
                }
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid amount. Try again.");
                Console.ResetColor();
            }
        }
    }
}