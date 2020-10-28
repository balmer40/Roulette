using Roulette.Models;
using Roulette.Validators;

namespace Roulette.Handlers
{
    public class CornerBetHandler : BetHandler
    {
        public CornerBetHandler(IBetTypeValidator betTypeValidator) : base(BetType.Corner, betTypeValidator)
        {
        }

        public override bool IsWinningBet(int winningNumber, int position = 0)
        {
            // as the position is the highest number in a corner bet, winning positions are within
            // four numbers except position - 2
            // e.g. position 12 would give winning positions 12, 11, 9, and 8, but not 10
            return winningNumber >= position - 4 &&
                   winningNumber <= position &&
                   winningNumber != position - 2;
        }

        public override WinningBet CalculateWinnings(Bet bet)
        {
            var amountWon = bet.Amount * BetMultipliers.CornerMultiplier;

            return CreateWinningBet(bet, amountWon);
        }
    }
}
