using Roulette.Exceptions;
using Roulette.Models;
using Roulette.Models.Requests;
using Roulette.Models.Responses;
using Roulette.Repositories;
using System;
using System.Threading.Tasks;

namespace Roulette.Services
{
    public class BetService : IBetService
    {
        private readonly IGameRepository _gameRepository;
        private readonly IBetRepository _betRepository;

        public BetService(
            IGameRepository gameRepository,
            IBetRepository betRepository)
        {
            _gameRepository = gameRepository;
            _betRepository = betRepository;
        }


        public async Task<AddBetResponse> AddBet(Guid gameId, AddBetRequest request)
        {
            await ValidateGameBettingIsOpen(gameId);

            var betId = await _betRepository.CreateBet(
                gameId,
                request.CustomerId,
                request.BetType,
                request.Position,
                request.Amount);

            return new AddBetResponse { BetId = betId };
        }

        public async Task UpdateBet(Guid gameId, Guid betId, UpdateBetRequest request)
        {
            await ValidateGameBettingIsOpen(gameId);

            await _betRepository.UpdateBet(betId, request.Amount);
        }

        public async Task DeleteBet(Guid gameId, Guid betId)
        {
            await ValidateGameBettingIsOpen(gameId);

            await _betRepository.DeleteBet(betId);
        }

        private async Task ValidateGameBettingIsOpen(Guid gameId)
        {
            var game = await _gameRepository.GetById(gameId);
            if (game.GameStatus == GameStatus.BettingClosed)
            {
                throw new GameBettingClosedException(game.Id);
            }
        }
    }
}
