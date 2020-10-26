using System;
using System.Threading.Tasks;

namespace Roulette.Services
{
    public interface IRouletteService
    {
        Task<Guid> CreateNewGame();

        Task CloseBets();
    }
}
