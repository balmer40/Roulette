using Roulette.Exceptions;
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
        private readonly IBetRepository _betRepository;

        public RouletteService(IGameRepository gameRepository, IBetRepository betRepository)
        {
            _gameRepository = gameRepository;
            _betRepository = betRepository;
        }

        public async Task<NewGameResponse> CreateNewGame()
        {
            var gameId = await _gameRepository.CreateGame();
            return new NewGameResponse { GameId = gameId };
        }

        public async Task CloseBets(Guid gameId)
        {
            await _gameRepository.CloseBets(gameId);
        }

        public async Task<AddBetResponse> AddBet(AddBetRequest request)
        {
            var game = await _gameRepository.GetById(request.GameId);
            if(!game.IsOpen)
            {
                throw new GameClosedException(request.GameId);
            }

            var betId = await _betRepository.CreateBet(
                request.GameId,
                request.CustomerId,
                request.BetType,
                request.Position,
                request.Amount);

            return new AddBetResponse { BetId = betId };
        }

        public async Task DeleteBet(Guid betId)
        {
            await _betRepository.DeleteBet(betId);
        }

        public Task<PlayGameResponse> PlayGame(Guid gameId)
        {
            throw new NotImplementedException();
        }
    }
}
