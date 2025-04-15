using BettyWallet.Domain.Entities;

namespace BettyWallet.Application.Interfaces
{
    /// <summary>
    /// Defines operations for saving and loading transaction history.
    /// </summary>
    public interface ITransactionHistoryService
    {
        /// <summary>
        /// Saves a transaction persistently.
        /// </summary>
        void SaveTransaction(Transaction transaction);

        /// <summary>
        /// Loads all stored transactions.
        /// </summary>
        List<Transaction> LoadTransactions();

        /// <summary>
        /// Clears the entire transaction history.
        /// </summary>
        void ClearHistory();
    }
}