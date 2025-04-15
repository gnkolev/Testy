using BettyWallet.Domain.Enums;
using System.Text.Json.Serialization;

namespace BettyWallet.Domain.Entities
{
    /// <summary>
    /// Represents a financial transaction (Deposit, Withdrawal, or Bet).
    /// </summary>
    public class Transaction
    {
        /// <summary>
        /// Unique identifier for the transaction.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// The type of transaction (Deposit, Withdrawal, Bet).
        /// </summary>
        public TransactionType Type { get; }

        /// <summary>
        /// The amount involved in the transaction.
        /// Always a positive value.
        /// </summary>
        public decimal Amount { get; }

        /// <summary>
        /// The date and time when the transaction occurred.
        /// Stored in UTC format for consistency.
        /// </summary>
        public DateTime Timestamp { get; }

        /// <summary>
        /// Creates a new transaction record.
        /// </summary>
        /// <param name="type">The type of transaction.</param>
        /// <param name="amount">The transaction amount.</param>
        [JsonConstructor]
        public Transaction(TransactionType type, decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Transaction amount must be greater than zero.");

            Id = Guid.NewGuid();
            Type = type;
            Amount = Math.Round(amount, 2);
            Timestamp = DateTime.Now;
        }
    }
}
