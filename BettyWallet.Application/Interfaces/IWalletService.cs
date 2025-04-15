using BettyWallet.Domain.Entities;

namespace BettyWallet.Application.Interfaces
{
    /// <summary>
    /// Defines operations related to wallet management.
    /// </summary>
    public interface IWalletService
    {
        void Deposit(Wallet wallet, decimal amount);
        bool Withdraw(Wallet wallet, decimal amount);
        decimal GetBalance(Wallet wallet);
        List<Transaction> GetTransactionHistory();
    }
}