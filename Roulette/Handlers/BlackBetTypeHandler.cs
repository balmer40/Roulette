using Roulette.Constants;
using Roulette.Models;
using Roulette.Validators;
using System.Linq;

namespace Roulette.Handlers
{
    public class BlackBetTypeHandler : BetTypeHandler
    {

        public BlackBetTypeHandler(IBetTypeValidator betTypeValidator)
            : base(BetType.Black, betTypeValidator, BetMultipliers.BlackAndRedMultiplier)
        {
        }

        public override bool IsWinningBet(int winningNumber, int position = 0)
        {
            return Numbers.BlackNumbers.Contains(winningNumber);
        }
    }
}
