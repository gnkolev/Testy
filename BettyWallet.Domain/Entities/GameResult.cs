using BettyWallet.Domain.Enums;

namespace BettyWallet.Domain.Entities
{
    /// <summary>
    /// Represents the outcome of a betting round.
    /// </summary>
    public class GameResult
    {
        /// <summary>
        /// The outcome of the bet (Lost, Win x2, Win x10).
        /// </summary>
        public BetResult Result { get; private set; }

        /// <summary>
        /// The amount the player won (0 if they lost).
        /// </summary>
        public decimal WinAmount { get; private set; }

        /// <summary>
        /// Creates a new game result record.
        /// </summary>
        /// <param name="result">The result of the bet.</param>
        /// <param name="winAmount">The amount won.</param>
        public GameResult(BetResult result, decimal winAmount)
        {
            Result = result;
            WinAmount = winAmount;
        }
    }
}
