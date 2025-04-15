using BettyWallet.Application.Interfaces;
using BettyWallet.Domain.Entities;
using BettyWallet.Domain.Enums;

namespace BettyWallet.Application.Services
{
    /// <summary>
    /// Handles wallet operations like deposits, withdrawals, and balance tracking.
    /// </summary>
    public class WalletService : IWalletService
    {
        private readonly ILoggerService _loggerService;
        private readonly ITransactionHistoryService _transactionHistoryService;

        public WalletService(ILoggerService loggerService, ITransactionHistoryService transactionHistoryService)
        {
            _loggerService = loggerService;
            _transactionHistoryService = transactionHistoryService;
        }

        public void Deposit(Wallet wallet, decimal amount)
        {
            if (wallet == null) throw new ArgumentNullException(nameof(wallet));
            wallet.Deposit(amount);

            var transaction = new Transaction(TransactionType.Deposit, amount);
            _transactionHistoryService.SaveTransaction(transaction);
            _loggerService.LogTransaction(transaction);
        }

        public bool Withdraw(Wallet wallet, decimal amount)
        {
            if (wallet == null) throw new ArgumentNullException(nameof(wallet));
            bool success = wallet.Withdraw(amount);

            if (success)
            {
                var transaction = new Transaction(TransactionType.Withdrawal, amount);
                _transactionHistoryService.SaveTransaction(transaction);
                _loggerService.LogTransaction(transaction);
            }

            return success;
        }

        public decimal GetBalance(Wallet wallet)
        {
            if (wallet == null) throw new ArgumentNullException(nameof(wallet));
            return wallet.Balance;
        }

        public List<Transaction> GetTransactionHistory()
        {
            return _transactionHistoryService.LoadTransactions();
        }
    }
}