using Roulette.Exceptions;
using Roulette.Models;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Roulette.Repositories
{
    // although the methods in this class don't do anything asynchronous, in reality they would with database calls,
    // so I have made the methods return a Task
    public class GameRepositoryStub : IGameRepository
    {
        private static readonly ConcurrentDictionary<Guid, Game> Games = new ConcurrentDictionary<Guid, Game>();

        public Task<Guid> CreateGame()
        {
            var gameId = Guid.NewGuid();
            var game = new Game
            {
                Id = gameId,
                OpenedAt = DateTime.Now
            };

            if(!Games.TryAdd(gameId, game))
            {
                throw new FailedToCreateGameException();
            }

            return Task.FromResult(gameId) ;
        }

        public async Task CloseBetting(Guid id)
        {
            var game = await GetById(id);
            var newGame = new Game
            {
                Id = game.Id,
                GameStatus = GameStatus.BettingClosed,
                OpenedAt = game.OpenedAt,
                ClosedAt = game.ClosedAt
            };

            if(!Games.TryUpdate(id, newGame, game))
            {
                throw new FailedToUpdateGameException(id);
            }
        }

        public async Task CloseGame(Guid id)
        {
            var game = await GetById(id);
            var newGame = new Game
            {
                Id = game.Id,
                GameStatus = GameStatus.GameClosed,
                OpenedAt = game.OpenedAt,
                ClosedAt = DateTime.Now
            };

            if (!Games.TryUpdate(id, newGame, game))
            {
                throw new FailedToUpdateGameException(id);
            }
        }

        public Task<Game> GetById(Guid id)
        {
            if (!Games.TryGetValue(id, out var game))
            {
                throw new GameNotFoundException(id);
            }

            return Task.FromResult(game);
        }
    }
}
