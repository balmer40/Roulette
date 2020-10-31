using Roulette.Handlers;
using Roulette.Models;
using System.Linq;
using Roulette.Exceptions;

namespace Roulette.Providers
{
    public class BetTypeHandlerProvider : IBetTypeHandlerProvider
    {
        private IBetTypeHandler[] _betTypeHandlers { get; }

        public BetTypeHandlerProvider(IBetTypeHandler[] betTypeHandlers)
        {
            _betTypeHandlers = betTypeHandlers;
        }

        public IBetTypeHandler GetBetTypeHandler(BetType betType)
        {
            var betTypeHandler = _betTypeHandlers.FirstOrDefault(handler => handler.BetType == betType);
            if (betTypeHandler == null)
            {
                throw new BetTypeNotConfiguredException(betType);
            }

            return betTypeHandler;
        }

    }
}
