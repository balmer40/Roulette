using Roulette.Exceptions;
using Roulette.Handlers;
using Roulette.Models;
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
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;
        private readonly IBetRepository _betRepository;
        private readonly ISpinWheelService _spinWheelService;
        private readonly IBetHandlerProvider _betHandlerProvider;

        public GameService(
            IGameRepository gameRepository,
            IBetRepository betRepository,
            ISpinWheelService spinWheelService,
            IBetHandlerProvider betHandlerProvider)
        {
            _gameRepository = gameRepository;
            _betRepository = betRepository;
            _spinWheelService = spinWheelService;
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
            await ValidateGameIsOpen(game);

            await _gameRepository.CloseBetting(gameId);
        }

        public async Task<PlayGameResponse> PlayGame(Guid gameId)
        {
            var game = await _gameRepository.GetById(gameId);
            await ValidateGameIsPlayable(game);

            var winningNumber = _spinWheelService.GetWinningNumber();
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
            var (customerWinningBets, customerLosingBets) = GetBetsByCustomer(betsByType, winningNumber);

            // a dictionary of customer ids along with their total winnings
            var customerTotalWinnings = customerWinningBets
                .ToDictionary(entry => entry.Key, entry => entry.Value.Sum(bet => bet.AmountWon));

            //at this stage, the winning bets data would be stored and/or ETLed, but I have not implemented this

            await _gameRepository.CloseGame(gameId);

            return new PlayGameResponse
            {
                GameId = gameId,
                WinningNumber = winningNumber,
                WinningBets = customerWinningBets,
                LosingBets = customerLosingBets,
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

        private (Dictionary<string, WinningBet[]>, Dictionary<string, LosingBet[]>) GetBetsByCustomer(Dictionary<BetType, ICollection<Bet>> betsByType, int winningNumber)
        {
            var customerWinningBets = new Dictionary<Guid, ICollection<WinningBet>>();
            var customerLosingBets = new Dictionary<Guid, ICollection<LosingBet>>();
            foreach (var (betType, bets) in betsByType)
            {
                var betHandler = _betHandlerProvider.GetBetHandler(betType);
                SortBets(bets, betHandler, winningNumber, customerWinningBets, customerLosingBets);
            }

            return (customerWinningBets.ToDictionary(entry => entry.Key.ToString(), entry => entry.Value.ToArray()),
                   customerLosingBets.ToDictionary(entry => entry.Key.ToString(), entry => entry.Value.ToArray()));
        }

        private void SortBets(
            IEnumerable<Bet> bets, 
            IBetHandler betHandler, 
            int winningNumber, 
            Dictionary<Guid, ICollection<WinningBet>> customerWinningBets,
            Dictionary<Guid, ICollection<LosingBet>> customerLosingBets)
        {
            foreach (var bet in bets)
            {
                if (betHandler.IsWinningBet(bet.Position, winningNumber))
                {
                    var winningBet = betHandler.CalculateWinnings(bet);
                    AddToCustomerWinningBets(bet.CustomerId, winningBet, customerWinningBets);
                }
                else
                {
                    AddToCustomerLosingBets(bet, customerLosingBets);
                }
            }
        }

        private void AddToCustomerWinningBets(Guid customerId, WinningBet winningBet, Dictionary<Guid, ICollection<WinningBet>> customerWinningBets)
        {
            AddToDictionary(customerId, winningBet, customerWinningBets);
        }

        private void AddToCustomerLosingBets(Bet bet, Dictionary<Guid, ICollection<LosingBet>> customerLosingBets)
        {
            var losingBet = new LosingBet
            {
                Id = bet.Id,
                BetType = bet.BetType,
                Position = bet.Position,
                AmountBet = bet.Amount
            };

            AddToDictionary(bet.CustomerId, losingBet, customerLosingBets);
        }

        private void AddToDictionary<T>(Guid customerId, T bet, Dictionary<Guid, ICollection<T>> customerBets)
        {
            if (customerBets.ContainsKey(customerId))
            {
                customerBets[customerId].Add(bet);
            }
            else
            {
                customerBets.Add(customerId, new Collection<T> { bet });
            }
        }

        private async Task ValidateGameIsOpen(Game game)
        {
            if (game.GameStatus == GameStatus.GameClosed)
            {
                throw new GameClosedException(game.Id);
            }
        }

        private async Task ValidateGameIsPlayable(Game game)
        {
            await ValidateGameIsOpen(game);

            if (game.GameStatus == GameStatus.GameOpen)
            {
                throw new GameBettingOpenException(game.Id);
            }
        }
    }
}
