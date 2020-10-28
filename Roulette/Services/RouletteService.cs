﻿using Roulette.Exceptions;
using Roulette.Handlers;
using Roulette.Models;
using Roulette.Models.Requests;
using Roulette.Models.Responses;
using Roulette.Providers;
using Roulette.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Roulette.Services
{
    public class RouletteService : IRouletteService
    {
        private readonly IGameRepository _gameRepository;
        private readonly IBetRepository _betRepository;
        private readonly IGameService _gameService;
        private readonly IBetHandlerProvider _betHandlerProvider;

        public RouletteService(
            IGameRepository gameRepository,
            IBetRepository betRepository,
            IGameService gameService,
            IBetHandlerProvider betHandlerProvider)
        {
            _gameRepository = gameRepository;
            _betRepository = betRepository;
            _gameService = gameService;
            _betHandlerProvider = betHandlerProvider;
        }

        public async Task<NewGameResponse> CreateNewGame()
        {
            var gameId = await _gameRepository.CreateGame();
            return new NewGameResponse { GameId = gameId };
        }

        public async Task CloseBetting(Guid gameId)
        {
            var game = await _gameRepository.GetById(gameId);
            if (game.GameStatus == GameStatus.GameClosed)
            {
                throw new GameClosedException(game.Id);
            }

            await _gameRepository.CloseBetting(gameId);
        }

        public async Task<AddBetResponse> AddBet(AddBetRequest request)
        {
            await ValidateGameBettingIsOpen(request.GameId);

            var betId = await _betRepository.CreateBet(
                request.GameId,
                request.CustomerId,
                request.BetType,
                request.Position,
                request.Amount);

            return new AddBetResponse { BetId = betId };
        }

        public async Task DeleteBet(DeleteBetRequest request)
        {
            await ValidateGameBettingIsOpen(request.GameId);

            await _betRepository.DeleteBet(request.BetId);
        }

        public async Task<PlayGameResponse> PlayGame(Guid gameId)
        {
            var game = await _gameRepository.GetById(gameId);
            if (game.GameStatus == GameStatus.GameOpen)
            {
                throw new GameBettingOpenException(gameId);
            }

            var winningNumber = _gameService.GetWinningNumber();

            var allBets = await _betRepository.GetAllBetsForGame(gameId);

            if (!allBets.Any())
            {
                //TODO log
                return new PlayGameResponse
                {
                    GameId = gameId,
                    WinningNumber = winningNumber,
                    WinningBets = new Dictionary<string, WinningBet[]>(),
                    CustomerTotalWinnings = new Dictionary<string, double>()
                };
            }

            var betsByType = GetBetsByType(allBets);

            var customerWinningBets = new Dictionary<Guid, ICollection<WinningBet>>();
            foreach (var (betType, bets) in betsByType)
            {
                var betHandler = _betHandlerProvider.GetBetHandler(betType);
                var winningBets = GetWinningBets(bets, betHandler, winningNumber);
                AddToCustomerWinningBets(winningBets, customerWinningBets);
            }

            // a dictionary of customer ids along with their total winnings
            var customerTotalWinnings = customerWinningBets
                .ToDictionary(entry => entry.Key.ToString(), entry => entry.Value.Sum(bet => bet.AmountWon));

            //at this stage, the winning bets data would be stored and/or ETLed, but I have not implemented this

            await _gameRepository.CloseGame(gameId);

            return new PlayGameResponse
            {
                GameId = gameId,
                WinningNumber = winningNumber,
                WinningBets = customerWinningBets
                    .ToDictionary(entry => entry.Key.ToString(), entry => entry.Value.ToArray()),
                CustomerTotalWinnings = customerTotalWinnings

            };
        }

        private Dictionary<BetType, ICollection<Bet>> GetBetsByType(ICollection<Bet> allBets)
        {
            var betsByType = new Dictionary<BetType, ICollection<Bet>>();
            foreach (var bet in allBets)
            {
                var betType = bet.BetType;
                if (betsByType.ContainsKey(betType))
                {
                    betsByType[betType].Add(bet);
                }
                else
                {
                    betsByType.Add(betType, new Collection<Bet> { bet });
                }
            }

            return betsByType;
        }

        private Dictionary<Guid, WinningBet> GetWinningBets(IEnumerable<Bet> bets, IBetHandler betHandler, int winningNumber)
        {
            var winningBets = new Dictionary<Guid, WinningBet>();
            foreach (var bet in bets)
            {
                if (betHandler.IsWinningBet(bet.Position, winningNumber))
                {
                    var winningBet = betHandler.CalculateWinnings(bet);
                    winningBets.Add(bet.CustomerId, winningBet);
                }
            }

            return winningBets;
        }

        private void AddToCustomerWinningBets(Dictionary<Guid, WinningBet> winnings, Dictionary<Guid, ICollection<WinningBet>> customerWinnings)
        {
            foreach (var (customerId, winningBet) in winnings)
            {
                if (customerWinnings.ContainsKey(customerId))
                {
                    customerWinnings[customerId].Add(winningBet);
                }
                else
                {
                    customerWinnings.Add(customerId, new Collection<WinningBet> { winningBet });
                }
            }
        }

        private async Task ValidateGameBettingIsOpen(Guid gameId)
        {
            var game = await _gameRepository.GetById(gameId);
            if (game.GameStatus == GameStatus.BettingClosed)
            {
                throw new GameBettingClosedException(game.Id);
            }
        }
    }
}
