using Roulette.Models;
using Roulette.Validators;
using System.Linq;
using Roulette.Constants;

namespace Roulette.Handlers
{
    public class RedBetTypeHandler : BetTypeHandler
    {
        public RedBetTypeHandler(IBetTypeValidator betTypeValidator) 
            : base(BetType.Red, betTypeValidator, BetMultipliers.BlackAndRedMultiplier)
        {
        }

        public override bool IsWinningBet(int winningNumber, int position = 0)
        {
            return Numbers.RedNumbers.Contains(winningNumber);
        }
    }
}
