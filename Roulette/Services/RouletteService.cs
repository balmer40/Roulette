using Roulette.Repositories;
using System;
using System.Threading.Tasks;

namespace Roulette.Services
{
    public class RouletteService : IRouletteService
    {
        private readonly IGameRepository _gameRepository;

        public RouletteService(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public async Task<Guid> CreateNewGame()
        {
            return await _gameRepository.CreateNewGame();
        }

        public async Task CloseBets()
        {
            throw new NotImplementedException();
        }
    }
}
