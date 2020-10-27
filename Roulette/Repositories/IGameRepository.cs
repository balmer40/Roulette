using Roulette.Models;
using System;
using System.Threading.Tasks;

namespace Roulette.Repositories
{
    public interface IGameRepository
    {
        Task<Guid> CreateNewGame();

        Task CloseBets(Guid gameId);
    }
}
