using Roulette.Models;
using Roulette.Validators;
using System.ComponentModel.DataAnnotations;

namespace Roulette.Handlers
{
    public interface IBetTypeHandler
    {
        BetType BetType { get; }

        IBetTypeValidator BetTypeValidator { get; }

        ValidationResult ValidatePosition(int position);

        bool IsWinningBet(int winningNumber, int position = 0);

        WinningBet CalculateWinnings(Bet bet);
    }
}
