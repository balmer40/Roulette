using Roulette.Models;
using Roulette.Validators;
using System.ComponentModel.DataAnnotations;

namespace Roulette.Handlers
{
    public abstract class BetHandler : IBetHandler
    {
        public BetType BetType { get; set; }
        public IBetTypeValidator BetTypeValidator { get; }

        public BetHandler(BetType betType, IBetTypeValidator betTypeValidator)
        {
            BetType = betType;
            BetTypeValidator = betTypeValidator;
        }

        public ValidationResult ValidatePosition(int position) => BetTypeValidator.ValidatePosition(position);

        public abstract bool IsWinningBet(int position, int winningNumber);
        public abstract WinningBet CalculateWinnings(Bet bet);

    }
}
