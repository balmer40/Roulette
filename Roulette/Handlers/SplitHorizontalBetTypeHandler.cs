using Roulette.Constants;
using Roulette.Models;
using Roulette.Validators;

namespace Roulette.Handlers
{
    public class SplitHorizontalBetTypeHandler : BetTypeHandler
    {

        public SplitHorizontalBetTypeHandler(IBetTypeValidator betTypeValidator) 
            : base(BetType.SplitHorizontal, betTypeValidator, BetMultipliers.SplitMultiplier)
        {
        }

        public override bool IsWinningBet(int winningNumber, int position = 0)
        {
            // as the position is the highest number in a split horizontal bet, winning positions are either
            // the position or the position - 1
            // e.g. position 6 would give 6 or 5 as winning positions
            return winningNumber == position ||
                winningNumber == position - 1;
        }
    }
}
