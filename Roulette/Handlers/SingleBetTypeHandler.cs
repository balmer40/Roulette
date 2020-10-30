using Roulette.Constants;
using Roulette.Models;
using Roulette.Validators;

namespace Roulette.Handlers
{
    public class SingleBetTypeHandler : BetTypeHandler
    {
        public SingleBetTypeHandler(IBetTypeValidator betTypeValidator) 
            : base(BetType.Single, betTypeValidator, BetMultipliers.SingleMultiplier)
        {
        }

        public override bool IsWinningBet(int winningNumber, int position = 0)
        {
            return winningNumber == position;
        }
    }
}
