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
    public class BetRepository : IBetRepository
    {
        private static readonly ConcurrentDictionary<Guid, Bet> _bets = new ConcurrentDictionary<Guid, Bet>();

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

            if(!_bets.TryAdd(betId, bet))
            {
                throw new FailedToCreateBetException();
            }

            return Task.FromResult(betId);
        }

        public Task DeleteBet(Guid id)
        {
            if(!_bets.ContainsKey(id))
            {
                throw new BetNotFoundException(id);
            }
            
            if(!_bets.TryRemove(id, out var _)) {
                throw new FailedToDeleteBetException(id);
            }

            return Task.CompletedTask;
        }

        //TODO remove
        public Task<Bet> GetById(Guid id)
        {
            if (!_bets.TryGetValue(id, out var bet))
            {
                throw new BetNotFoundException(id);
            }

            return Task.FromResult(bet);
        }

        public Task<Bet[]> GetAll(Guid gameId)
        {
            throw new NotImplementedException();
        }
    }
}
