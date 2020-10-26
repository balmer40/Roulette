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
            // TODO check for null and throw exception
            return _betHandlers.FirstOrDefault(betHandler => betHandler.BetType == betType);
        }
        
    }
}
