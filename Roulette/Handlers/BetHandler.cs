using Roulette.Models;
using Roulette.Validators;
using System.ComponentModel.DataAnnotations;

namespace Roulette.Handlers
{
    public abstract class BetHandler : IBetHandler
    {
        public BetType BetType { get; }
        public IBetTypeValidator BetTypeValidator { get; }

        public BetHandler(BetType betType, IBetTypeValidator betTypeValidator)
        {
            BetType = betType;
            BetTypeValidator = betTypeValidator;
        }

        public ValidationResult ValidateBetTypeAndPosition(BetType betType, int position)
        {
            return BetTypeValidator.ValidateBetTypeAndPosition(betType, position);
        }

        public abstract bool IsWinningBet();
        public abstract string CalculateWinnings();

    }
}
