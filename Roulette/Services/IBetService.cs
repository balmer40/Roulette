using Roulette.Models.Requests;
using Roulette.Models.Responses;
using System;
using System.Threading.Tasks;

namespace Roulette.Services
{
    public interface IBetService
    {
        Task<AddBetResponse> AddBet(Guid gameId, AddBetRequest request);

        Task UpdateBet(Guid gameId, Guid betId, UpdateBetRequest request);

        Task DeleteBet(Guid gameId, Guid betId);
    }
}
