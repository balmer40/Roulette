using Roulette.Models;
using System.ComponentModel.DataAnnotations;

namespace Roulette.Handlers
{
    public interface IBetTypeHandler
    {
        BetType BetType { get; }

        ValidationResult ValidatePosition(int? position);

        bool IsWinningBet(int winningNumber, int position = 0);

        WinningBet CalculateWinnings(Bet bet);
    }
}
