using Roulette.Models.Requests;
using Roulette.Models.Responses;
using System;
using System.Threading.Tasks;

namespace Roulette.Services
{
    public interface IRouletteService
    {
        Task<NewGameResponse> CreateNewGame();

        Task CloseBets(Guid gameId);

        Task<AddBetResponse> AddBet(AddBetRequest request);

        Task DeleteBet(Guid betId);

        Task<PlayGameResponse> PlayGame(Guid gameId);
    }
}
