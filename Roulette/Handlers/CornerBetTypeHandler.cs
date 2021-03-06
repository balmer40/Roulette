﻿using Roulette.Constants;
using Roulette.Models;
using Roulette.Validators;

namespace Roulette.Handlers
{
    public class CornerBetTypeHandler : BetTypeHandler
    {
        public CornerBetTypeHandler(IBetTypeValidator betTypeValidator) 
            : base(BetType.Corner, betTypeValidator, BetMultipliers.CornerMultiplier)
        {
        }

        public override bool IsWinningBet(int winningNumber, int position = 0)
        {
            // as the position is the highest number in a corner bet, winning positions are within
            // four numbers except position - 2
            // e.g. position 12 would give winning positions 12, 11, 9, and 8, but not 10
            return position >= winningNumber - 4 &&
                   position <= winningNumber &&
                   winningNumber != position - 2;
        }
    }
}
