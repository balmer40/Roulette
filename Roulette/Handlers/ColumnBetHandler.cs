using Roulette.Models;
using Roulette.Validators;
using System.Linq;

namespace Roulette.Handlers
{
    public class ColumnBetHandler : BetHandler
    {
        public ColumnBetHandler(IBetTypeValidator betTypeValidator) : base(BetType.Column, betTypeValidator)
        {
        }

        public override bool IsWinningBet(int winningNumber, int position = 0)
        {
            //the position represents the column number
            switch (position)
            {
                case 1: return Numbers.FirstColumnNumbers.Contains(winningNumber);
                case 2: return Numbers.SecondColumnNumbers.Contains(winningNumber);
                case 3: return Numbers.ThirdColumnNumbers.Contains(winningNumber);
                default:
                    break;
            }

            return false;
        }

        public override WinningBet CalculateWinnings(Bet bet)
        {
            var amountWon = bet.Amount * BetMultipliers.ColumnMultiplier;

            return CreateWinningBet(bet, amountWon);
        }
    }
}
