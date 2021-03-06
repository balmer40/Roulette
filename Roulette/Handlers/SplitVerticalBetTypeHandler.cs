﻿using Roulette.Constants;
using Roulette.Models;
using Roulette.Validators;

namespace Roulette.Handlers
{
    public class SplitVerticalBetTypeHandler : BetTypeHandler
    {

        public SplitVerticalBetTypeHandler(IBetTypeValidator betTypeValidator) 
            : base(BetType.SplitVertical, betTypeValidator, BetMultipliers.SplitMultiplier)
        {
        }

        public override bool IsWinningBet(int winningNumber, int position = 0)
        {
            // as the position is the highest number in a split vertical bet, winning positions are either
            // the position or the position - 3
            // e.g. position 6 would give 6 or 3 as winning positions
            // the exception to this is for positions 1 and 2 where the winning positions are either the position or 0

            if(position == 1 || position == 2)
            {
                return winningNumber == position ||
                       winningNumber == 0;
            }

            return winningNumber == position ||
                winningNumber == position - 3;
        }
    }
}
