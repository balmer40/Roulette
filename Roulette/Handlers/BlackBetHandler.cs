using Roulette.Models;
using Roulette.Validators;
using System.Linq;

namespace Roulette.Handlers
{
    public class BlackBetHandler : BetHandler
    {

        public BlackBetHandler(IBetTypeValidator betTypeValidator) : base(BetType.Black, betTypeValidator)
        {
        }

        public override bool IsWinningBet(int winningNumber, int position = 0)
        {
            return Numbers.BlackNumbers.Contains(winningNumber);
        }

        public override WinningBet CalculateWinnings(Bet bet)
        {
            var amountWon = bet.Amount * BetMultipliers.BlackAndRedMultiplier;

            return CreateWinningBet(bet, amountWon);
        }
    }
}
