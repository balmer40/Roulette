using Roulette.Models;
using Roulette.Validators;
using System.Linq;

namespace Roulette.Handlers
{
    public class RedBetHandler : BetHandler
    {
        public RedBetHandler(IBetTypeValidator betTypeValidator) : base(BetType.Red, betTypeValidator)
        {
        }

        public override bool IsWinningBet(int winningNumber, int position = 0)
        {
            return Numbers.RedNumbers.Contains(winningNumber);
        }

        public override WinningBet CalculateWinnings(Bet bet)
        {
            var amountWon = bet.Amount * BetMultipliers.BlackAndRedMultiplier;

            return CreateWinningBet(bet, amountWon);
        }
    }
}
