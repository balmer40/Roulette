using Roulette.Exceptions;
using Roulette.Models;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Roulette.Repositories
{
    // although the methods in this class don't do anything asynchronous, in reality they would with database calls,
    // so I have made the methods return a Task
    public class GameRepository : IGameRepository
    {
        private readonly ConcurrentDictionary<Guid, Game> _games = new ConcurrentDictionary<Guid, Game>();

        public Task<Guid> CreateNewGame()
        {
            var gameId = Guid.NewGuid();
            var game = new Game
            {
                GameId = gameId,
                OpenedAt = DateTime.Now
            };

            //TODO exception if fails?
            _games.TryAdd(gameId, game);

            return Task.FromResult(game.GameId);
        }

        public async Task CloseBets(Guid id)
        {
            var game = await GetById(id);
            var newGame = new Game
            {
                GameId = game.GameId,
                IsOpen = false,
                OpenedAt = game.OpenedAt,
                ClosedAt = DateTime.Now
            };

            //TODO exception if fails?
            _games.TryUpdate(id, newGame, game);
        }

        public async Task<Game> GetById(Guid id)
        {
            if (!_games.TryGetValue(id, out var game))
            {
                //TODO log
                throw new GameNotFoundException(id);
            }

            return game;
        }
    }
}
