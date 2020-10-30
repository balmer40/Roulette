using Roulette.Handlers;
using Roulette.Models;

namespace Roulette.Providers
{
    public interface IBetTypeHandlerProvider
    {
        IBetTypeHandler GetBetTypeHandler(BetType betType);
    }
}
