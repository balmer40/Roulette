using Roulette.Models;
using System;
using System.Threading.Tasks;

namespace Roulette.Repositories
{
    public interface IBetRepository
    {
        Task<Bet> CreateBet(Guid gameId, Guid customerId, BetType betType, int? position, double amount);

        Task<Bet> UpdateBet(Guid id, double amount);

        Task DeleteBet(Guid id);

        Task<Bet[]> GetAllBetsForGame(Guid gameId);
    }
}
