using Roulette.Models.Requests;
using Roulette.Models.Responses;
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

        public Task CloseBets(CloseBetsRequest request)
        {
            throw new NotImplementedException();
        }

        public Task AddBet(AddBetRequest request)
        {
            throw new NotImplementedException();
        }

        public Task RemoveBet(RemoveBetRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<PlayGameResponse> PlayGame(PlayGameRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
