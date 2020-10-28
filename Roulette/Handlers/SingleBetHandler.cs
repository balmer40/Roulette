using Roulette.Models;
using Roulette.Validators;

namespace Roulette.Handlers
{
    public class SingleBetHandler : BetHandler
    {
        public SingleBetHandler(IBetTypeValidator betTypeValidator) : base(BetType.Single, betTypeValidator)
        {
        }

        public override bool IsWinningBet(int position, int winningNumber)
        {
            return position == winningNumber;
        }

        public override WinningBet CalculateWinnings(Bet bet)
        {
            var amountWon = bet.Amount * 36;

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
