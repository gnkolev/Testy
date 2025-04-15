namespace BettyWallet.Domain.Entities
{
    /// <summary>
    /// Represents a player in the system.
    /// Each player has a unique identifier and an associated wallet.
    /// </summary>
    public class Player
    {
        /// <summary>
        /// Unique identifier for the player.
        /// Helps to track and differentiate players.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// The player's wallet which holds their balance.
        /// Every player has their own wallet.
        /// </summary>
        public Wallet Wallet { get; private set; }

        /// <summary>
        /// Initializes a new player with a unique ID and an empty wallet.
        /// </summary>
        public Player()
        {
            Id = Guid.NewGuid();
            Wallet = new Wallet();
        }
    }
}