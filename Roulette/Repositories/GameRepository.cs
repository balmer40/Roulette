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

            _games.TryAdd(gameId, game);

            return Task.FromResult(game.GameId);
        }

        public async Task CloseBets()
        {
            throw new NotImplementedException();
        }

        public async Task<Game> GetById(Guid id)
        {
            if (!_games.TryGetValue(id, out var game))
            {
                //TODO
                throw new Exception();
            }

            return game;
        }
    }
}
