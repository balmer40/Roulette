﻿using Roulette.Models;
using Roulette.Validators;

namespace Roulette.Handlers
{
    public class SingleBetHandler : BetHandler
    {
        public SingleBetHandler(IBetTypeValidator betTypeValidator) : base(BetType.Single, betTypeValidator)
        {
        }

        public override bool IsWinningBet(int winningNumber, int position = 0)
        {
            return winningNumber == position;
        }

        public override WinningBet CalculateWinnings(Bet bet)
        {
            var amountWon = bet.Amount * BetMultipliers.SingleMultiplier;

            return CreateWinningBet(bet, amountWon);
        }
    }
}
