using Roulette.Exceptions;
using Roulette.Models;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Roulette.Repositories
{
    // although the methods in this class don't do anything asynchronous, in reality they would with database calls,
    // so I have made the methods return a Task

    //TODO locks
    public class GameRepository : IGameRepository
    {
        private static readonly ConcurrentDictionary<Guid, Game> _games = new ConcurrentDictionary<Guid, Game>();

        public Task<Guid> CreateGame()
        {
            var gameId = Guid.NewGuid();
            var game = new Game
            {
                Id = gameId,
                OpenedAt = DateTime.Now
            };

            if(!_games.TryAdd(gameId, game))
            {
                throw new FailedToCreateGameException();
            }

            return Task.FromResult(gameId) ;
        }

        public async Task CloseBets(Guid id)
        {
            var game = await GetById(id);
            var newGame = new Game
            {
                Id = game.Id,
                IsOpen = false,
                OpenedAt = game.OpenedAt,
                ClosedAt = DateTime.Now
            };

            if(!_games.TryUpdate(id, newGame, game))
            {
                throw new FailedToUpdateGameException(id);
            }
        }

        public Task<Game> GetById(Guid id)
        {
            if (!_games.TryGetValue(id, out var game))
            {
                throw new GameNotFoundException(id);
            }

            return Task.FromResult(game);
        }
    }
}
