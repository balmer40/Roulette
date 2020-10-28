using Roulette.Models;
using System;
using System.Threading.Tasks;

namespace Roulette.Repositories
{
    public interface IGameRepository
    {
        Task<Guid> CreateGame();

        Task CloseBets(Guid gameId);

        Task<Game> GetById(Guid id);
    }
}
