using Roulette.Models;
using Roulette.Validators;
using System;

namespace Roulette.Handlers
{
    public class SingleBetHandler : BetHandler
    {
        public SingleBetHandler(IBetTypeValidator betTypeValidator) : base(BetType.Single, betTypeValidator)
        {
        }

        public override bool IsWinningBet()
        {
            throw new NotImplementedException();
        }

        public override string CalculateWinnings()
        {
            throw new NotImplementedException();
        }
    }
}
