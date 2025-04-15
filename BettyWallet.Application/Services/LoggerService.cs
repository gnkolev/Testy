using BettyWallet.Application.Interfaces;
using BettyWallet.Domain.Entities;

namespace BettyWallet.Application.Services
{
    /// <summary>
    /// Service for logging transactions and game results to a file.
    /// </summary>
    public class LoggerService : ILoggerService
    {
        private readonly string _logFilePath;
        private readonly object _lock = new object();

        public LoggerService(string logFilePath = "wallet_log.txt")
        {
            _logFilePath = logFilePath;
            EnsureLogFileExists();
        }

        private void EnsureLogFileExists()
        {
            try
            {
                if (!File.Exists(_logFilePath))
                {
                    using (File.Create(_logFilePath)) { }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Logger initialization error: {ex.Message}");
            }
        }

        public void LogTransaction(Transaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction), "Transaction cannot be null");

            string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | TRANSACTION | {transaction.Type} | Amount: ${transaction.Amount:F2}";
            WriteLog(logEntry);
        }

        public void LogGameResult(GameResult result, decimal betAmount)
        {
            if (result == null) throw new ArgumentNullException(nameof(result), "Game result cannot be null");

            string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | GAME RESULT | Bet: ${betAmount:F2} | Outcome: {result.Result} | Won: ${result.WinAmount:F2}";
            WriteLog(logEntry);
        }
        private void WriteLog(string logEntry)
        {
            try
            {
                lock (_lock)
                {
                    using (StreamWriter writer = new StreamWriter(_logFilePath, true))
                    {
                        writer.WriteLine(logEntry);
                        writer.Flush();
                    }
                }
            }
            catch (IOException ioEx)
            {
                Console.WriteLine($"I/O error while writing to log file: {ioEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while writing log: {ex.Message}");
            }
        }
    }
}