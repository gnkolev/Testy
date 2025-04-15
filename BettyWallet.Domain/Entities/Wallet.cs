namespace BettyWallet.Domain.Entities
{
    /// <summary>
    /// Represents a player's wallet.
    /// This manages the balance and allows deposits & withdrawals.
    /// </summary>
    public class Wallet
    {
        /// <summary>
        /// The balance in the player's wallet.
        /// Should always be a non-negative value.
        /// </summary>
        public decimal Balance { get; private set; }

        /// <summary>
        /// Initializes a new wallet with a zero balance.
        /// </summary>
        public Wallet()
        {
            Balance = 0;
        }

        /// <summary>
        /// Adds money to the wallet.
        /// </summary>
        /// <param name="amount">The amount to deposit (must be positive).</param>
        public void Deposit(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Deposit amount must be greater than zero.");

            Balance += amount;
        }

        /// <summary>
        /// Withdraws money from the wallet if sufficient balance exists.
        /// </summary>
        /// <param name="amount">The amount to withdraw.</param>
        /// <returns>True if withdrawal is successful, otherwise false.</returns>
        public bool Withdraw(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Withdrawal amount must be greater than zero.");

            if (amount > Balance)
                return false;

            Balance -= amount;
            return true;
        }
    }
}
