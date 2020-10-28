using Roulette.Models;
using System;
using System.Threading.Tasks;

namespace Roulette.Repositories
{
    public interface IGameRepository
    {
        Task<Guid> CreateGame();

        Task CloseBetting(Guid id);

        Task CloseGame(Guid id);

        Task<Game> GetById(Guid id);
    }
}
