using System;
using Roulette.Models;
using Roulette.Validators;
using System.ComponentModel.DataAnnotations;

namespace Roulette.Handlers
{
    public abstract class BetTypeHandler : IBetTypeHandler
    {
        private readonly int _betMultiplier;
        public BetType BetType { get; set; }
        public IBetTypeValidator BetTypeValidator { get; }

        protected BetTypeHandler(BetType betType, IBetTypeValidator betTypeValidator, int betMultiplier)
        {
            _betMultiplier = betMultiplier;
            BetType = betType;
            BetTypeValidator = betTypeValidator;
        }

        public ValidationResult ValidatePosition(int? position) => BetTypeValidator.ValidatePosition(position);

        public WinningBet CalculateWinnings(Bet bet)
        {
            var amountWon = Math.Round(bet.Amount * _betMultiplier, 2);

            return new WinningBet
            {
                Id = bet.Id,
                BetType = bet.BetType,
                Position = bet.Position,
                AmountBet = bet.Amount,
                AmountWon = amountWon
            };
        }

        public abstract bool IsWinningBet(int winningNumber, int position = 0);
    }
}
