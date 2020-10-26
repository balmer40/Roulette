using Roulette.Models;
using Roulette.Validators;
using System.ComponentModel.DataAnnotations;

namespace Roulette.Handlers
{
    public interface IBetHandler
    {
        BetType BetType { get; }

        IBetTypeValidator BetTypeValidator { get; }

        ValidationResult ValidateBetTypeAndPosition(BetType betType, int position);

        bool IsWinningBet();

        string CalculateWinnings();
    }
}
