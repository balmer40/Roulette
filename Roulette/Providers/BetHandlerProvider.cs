using Roulette.Handlers;
using Roulette.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Roulette.Providers
{
    public class BetHandlerProvider: IBetHandlerProvider
    {
        private IBetHandler[] _betHandlers { get; }

        public BetHandlerProvider(IBetHandler[] betHandlers)
        {
            _betHandlers = betHandlers;
        }

        public IBetHandler GetBetHandler(BetType betType)
        {
            return _betHandlers.First(betHandler => betHandler.BetType == betType);
        }
        
    }
}
