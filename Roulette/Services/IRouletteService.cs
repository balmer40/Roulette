using Roulette.Models.Requests;
using Roulette.Models.Responses;
using System;
using System.Threading.Tasks;

namespace Roulette.Services
{
    public interface IRouletteService
    {
        Task<Guid> CreateNewGame();

        Task CloseBets(CloseBetsRequest request);

        Task AddBet(AddBetRequest request);

        Task RemoveBet(RemoveBetRequest request);

        Task<PlayGameResponse> PlayGame(PlayGameRequest request);
    }
}
