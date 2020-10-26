using Roulette.Handlers;
using Roulette.Models;

namespace Roulette.Providers
{
    public interface IBetHandlerProvider
    {
        IBetHandler GetBetHandler(BetType betType);
    }
}
