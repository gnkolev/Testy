using BettyWallet.Domain.Entities;

namespace BettyWallet.Application.Interfaces
{
    /// <summary>
    /// Interface for logging transactions and game results.
    /// </summary>
    public interface ILoggerService
    {
        void LogTransaction(Transaction transaction);
        void LogGameResult(GameResult result, decimal betAmount);
    }
}
