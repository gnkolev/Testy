using BettyWallet.Domain.Entities;

namespace BettyWallet.Application.Interfaces
{
    /// <summary>
    /// Defines game-related operations.
    /// </summary>
    public interface IGameService
    {
        GameResult Play(Player player, decimal betAmount);
    }
}