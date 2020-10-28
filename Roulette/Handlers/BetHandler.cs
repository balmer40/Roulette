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

        public ValidationResult ValidatePosition(int position = 0) => BetTypeValidator.ValidatePosition(position);

        public abstract bool IsWinningBet(int winningNumber, int position = 0);
        public abstract WinningBet CalculateWinnings(Bet bet);

        protected WinningBet CreateWinningBet(Bet bet, double amountWon)
        {
            return new WinningBet
            {
                Id = bet.Id,
                BetType = bet.BetType,
                Position = bet.Position,
                AmountBet = bet.Amount,
                AmountWon = amountWon
            };
        }

    }
}
