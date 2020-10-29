using Roulette.Exceptions;
using Roulette.Models;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace Roulette.Repositories
{
    // although the methods in this class don't do anything asynchronous, in reality they would with database calls,
    // so I have made the methods return a Task
    public class BetRepositoryStub : IBetRepository
    {
        private static readonly ConcurrentDictionary<Guid, Bet> Bets = new ConcurrentDictionary<Guid, Bet>();

        public Task<Guid> CreateBet(Guid gameId, Guid customerId, BetType betType, int position, double amount)
        {
            var betId = Guid.NewGuid();
            var bet = new Bet
            {
                Id = betId,
                GameId = gameId,
                CustomerId = customerId,
                BetType = betType,
                Position = position,
                Amount = amount
            };

            if (!Bets.TryAdd(betId, bet))
            {
                throw new FailedToCreateBetException(gameId);
            }

            return Task.FromResult(betId);
        }

        public Task UpdateBet(Guid id, double amount)
        {
            if(!Bets.TryGetValue(id, out var bet))
            {
                throw new BetNotFoundException(id);
            }

            if(bet.Amount + amount > Ranges.MaximumBet)
            {
                throw new UpdateAmountTooHighException(id);
            }

            var newBet = new Bet
            {
                Id = bet.Id,
                GameId = bet.GameId,
                CustomerId = bet.CustomerId,
                BetType = bet.BetType,
                Position = bet.Position,
                Amount = bet.Amount + amount
            };

            if (!Bets.TryUpdate(id, newBet, bet))
            {
                throw new FailedToUpdateBetException(id);
            }

            return Task.CompletedTask;
        }

        public Task DeleteBet(Guid id)
        {
            if (!Bets.ContainsKey(id))
            {
                throw new BetNotFoundException(id);
            }

            if (!Bets.TryRemove(id, out _))
            {
                throw new FailedToDeleteBetException(id);
            }

            return Task.CompletedTask;
        }

        public Task<Bet[]> GetAllBetsForGame(Guid gameId)
        {
            var bets = Bets
                .Where(bet => bet.Value.GameId == gameId)
                .Select(bet => bet.Value);

            return Task.FromResult(bets.ToArray());
        }
    }
}
