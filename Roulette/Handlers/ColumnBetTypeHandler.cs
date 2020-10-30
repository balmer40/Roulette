using Roulette.Models;
using Roulette.Validators;
using System.Linq;
using Roulette.Constants;

namespace Roulette.Handlers
{
    public class ColumnBetTypeHandler : BetTypeHandler
    {
        public ColumnBetTypeHandler(IBetTypeValidator betTypeValidator) 
            : base(BetType.Column, betTypeValidator, BetMultipliers.ColumnMultiplier)
        {
        }

        public override bool IsWinningBet(int winningNumber, int position = 0)
        {
            //the position represents the column number
            return position switch
            {
                1 => Numbers.FirstColumnNumbers.Contains(winningNumber),
                2 => Numbers.SecondColumnNumbers.Contains(winningNumber),
                3 => Numbers.ThirdColumnNumbers.Contains(winningNumber),
                _ => false
            };
        }
    }
}
