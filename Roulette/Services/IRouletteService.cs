﻿using Roulette.Models.Requests;
using Roulette.Models.Responses;
using System;
using System.Threading.Tasks;

namespace Roulette.Services
{
    public interface IRouletteService
    {
        Task<NewGameResponse> CreateNewGame();

        Task CloseBetting(Guid gameId);

        Task<PlayGameResponse> PlayGame(Guid gameId);
    }
}
