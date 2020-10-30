using Roulette.Models;
using System;

namespace Roulette.Exceptions
{
    public class BetTypeNotConfiguredException : Exception
    {
        public BetTypeNotConfiguredException(BetType betType) : base($"BetType not configured: {betType.ToString()}")
        {

        }
    }
}
