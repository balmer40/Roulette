using FluentAssertions;
using NSubstitute;
using Roulette.Exceptions;
using Roulette.Handlers;
using Roulette.Models;
using Roulette.Models.Requests;
using Roulette.Models.Responses;
using Roulette.Providers;
using Roulette.Repositories;
using Roulette.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Roulette.Tests.Services
{
    public class RouletteServiceTest
    {
        #region CreateNewGame

        [Fact]
        public async Task CreateNewGame_CreatesNewGame()
        {
            var mockRepository = Substitute.For<IGameRepository>();
            var service = new RouletteService(
                mockRepository,
                Substitute.For<IBetRepository>(),
                Substitute.For<IGameService>(),
                Substitute.For<IBetHandlerProvider>());

            await service.CreateNewGame();
            await mockRepository.Received().CreateGame();
        }

        [Fact]
        public async Task CreateNewGame_ReturnsNewGameResponse()
        {
            var expectedGameId = Guid.NewGuid();
            var mockRepository = Substitute.For<IGameRepository>();
            mockRepository.CreateGame().Returns(expectedGameId);
            var service = new RouletteService(
                mockRepository,
                Substitute.For<IBetRepository>(),
                Substitute.For<IGameService>(),
                Substitute.For<IBetHandlerProvider>());

            var result = await service.CreateNewGame();

            result.Should().BeOfType(typeof(NewGameResponse));
            result.GameId.Should().Be(expectedGameId);
        }

        #endregion

        #region CloseBetting

        [Fact]
        public async Task CloseBetting_ClosesBettingOnGame()
        {
            var expectedGameId = Guid.NewGuid();
            var mockRepository = Substitute.For<IGameRepository>();
            mockRepository.GetById(Arg.Any<Guid>()).Returns(new Game ());

            var service = new RouletteService(
                mockRepository,
                Substitute.For<IBetRepository>(),
                Substitute.For<IGameService>(),
                Substitute.For<IBetHandlerProvider>());

            await service.CloseBetting(expectedGameId);

            await mockRepository.Received().CloseBetting(expectedGameId);
        }

        [Fact]
        public async Task CloseBetting_ThrowsWhenGameIsNotOpen()
        {
            var expectedGameId = Guid.NewGuid();
            var mockRepository = Substitute.For<IGameRepository>();
            mockRepository.GetById(Arg.Any<Guid>()).Returns(new Game { GameStatus = GameStatus.GameClosed });
            var service = new RouletteService(
                mockRepository,
                Substitute.For<IBetRepository>(),
                Substitute.For<IGameService>(),
                Substitute.For<IBetHandlerProvider>());

            await Assert.ThrowsAsync<GameClosedException>(() => service.CloseBetting(expectedGameId));
        }

        #endregion

        #region AddBet

        [Fact]
        public async Task AddBet_CreatesBet()
        {
            var expectedGameId = Guid.NewGuid();
            var expectedCustomerId = Guid.NewGuid();
            var expectedBetType = BetType.Single;
            var expectedPosition = 1;
            var expectedAmount = 50.0;

            var mockGameRepository = Substitute.For<IGameRepository>();
            mockGameRepository.GetById(Arg.Any<Guid>()).Returns(new Game());
            var mockBetRepository = Substitute.For<IBetRepository>();
            var service = new RouletteService(
                mockGameRepository,
                mockBetRepository,
                Substitute.For<IGameService>(),
                Substitute.For<IBetHandlerProvider>());

            await service.AddBet(
                new AddBetRequest
                {
                    GameId = expectedGameId,
                    CustomerId = expectedCustomerId,
                    BetType = expectedBetType,
                    Position = expectedPosition,
                    Amount = expectedAmount
                });

            await mockBetRepository.Received().CreateBet(
                expectedGameId,
                expectedCustomerId,
                expectedBetType,
                expectedPosition,
                expectedAmount);
        }

        [Fact]
        public async Task AddBet_ThrowsWhenGameIsClosed()
        {
            var mockRepository = Substitute.For<IGameRepository>();
            mockRepository.GetById(Arg.Any<Guid>()).Returns(new Game { GameStatus = GameStatus.GameClosed });

            var service = new RouletteService(
                mockRepository,
                Substitute.For<IBetRepository>(),
                Substitute.For<IGameService>(),
                Substitute.For<IBetHandlerProvider>());

            await Assert.ThrowsAsync<GameClosedException>(() => service.AddBet(new AddBetRequest()));
        }

        [Fact]
        public async Task AddBet_ThrowsWhenBettingIsClosed()
        {
            var mockRepository = Substitute.For<IGameRepository>();
            mockRepository.GetById(Arg.Any<Guid>()).Returns(new Game { GameStatus = GameStatus.BettingClosed });

            var service = new RouletteService(
                mockRepository,
                Substitute.For<IBetRepository>(),
                Substitute.For<IGameService>(),
                Substitute.For<IBetHandlerProvider>());

            await Assert.ThrowsAsync<GameBettingClosedException>(() => service.AddBet(new AddBetRequest()));
        }

        [Fact]
        public async Task AddBet_ReturnsBetId()
        {
            var expectedBetId = Guid.NewGuid();
            var mockGameRepository = Substitute.For<IGameRepository>();
            mockGameRepository.GetById(Arg.Any<Guid>()).Returns(new Game());
            var mockBetRepository = Substitute.For<IBetRepository>();
            mockBetRepository.CreateBet(Arg.Any<Guid>(), Arg.Any<Guid>(), Arg.Any<BetType>(), Arg.Any<int>(), Arg.Any<double>())
                .Returns(expectedBetId);

            var service = new RouletteService(
                mockGameRepository,
                mockBetRepository,
                Substitute.For<IGameService>(),
                Substitute.For<IBetHandlerProvider>());

            var result = await service.AddBet(new AddBetRequest());

            result.Should().BeOfType(typeof(AddBetResponse));
            result.BetId.Should().Be(expectedBetId);
        }

        #endregion

        #region DeleteBet

        [Fact]
        public async Task DeleteBet_DeletesBet()
        {
            var expectedBetId = Guid.NewGuid();
            var mockGameRepository = Substitute.For<IGameRepository>();
            mockGameRepository.GetById(Arg.Any<Guid>()).Returns(new Game());
            var mockBetRepository = Substitute.For<IBetRepository>();
            var service = new RouletteService(
                mockGameRepository,
                mockBetRepository,
                Substitute.For<IGameService>(),
                Substitute.For<IBetHandlerProvider>());

            await service.DeleteBet(new DeleteBetRequest { BetId = expectedBetId });

            await mockBetRepository.Received().DeleteBet(expectedBetId);
        }

        [Fact]
        public async Task DeleteBet_ThrowsWhenGameIsClosed()
        {
            var mockRepository = Substitute.For<IGameRepository>();
            mockRepository.GetById(Arg.Any<Guid>()).Returns(new Game { GameStatus = GameStatus.GameClosed });

            var service = new RouletteService(
                mockRepository,
                Substitute.For<IBetRepository>(),
                Substitute.For<IGameService>(),
                Substitute.For<IBetHandlerProvider>());

            await Assert.ThrowsAsync<GameClosedException>(() => service.DeleteBet(new DeleteBetRequest()));
        }

        [Fact]
        public async Task DeleteBet_ThrowsWhenBettingIsClosed()
        {
            var mockRepository = Substitute.For<IGameRepository>();
            mockRepository.GetById(Arg.Any<Guid>()).Returns(new Game { GameStatus = GameStatus.BettingClosed });

            var service = new RouletteService(
                mockRepository,
                Substitute.For<IBetRepository>(),
                Substitute.For<IGameService>(),
                Substitute.For<IBetHandlerProvider>());

            await Assert.ThrowsAsync<GameBettingClosedException>(() => service.DeleteBet(new DeleteBetRequest()));
        }

        #endregion

        #region PlayGame

        [Fact]
        public async Task PlayGame_ThrowsWhenGameClosed()
        {
            var mockRepository = Substitute.For<IGameRepository>();
            mockRepository.GetById(Arg.Any<Guid>()).Returns(new Game { GameStatus = GameStatus.GameClosed });

            var service = new RouletteService(
                mockRepository,
                Substitute.For<IBetRepository>(),
                Substitute.For<IGameService>(),
                Substitute.For<IBetHandlerProvider>());

            await Assert.ThrowsAsync<GameClosedException>(() => service.AddBet(new AddBetRequest()));
        }

        [Fact]
        public async Task PlayGame_ThrowsWhenBetsStillOpen()
        {
            var mockRepository = Substitute.For<IGameRepository>();
            mockRepository.GetById(Arg.Any<Guid>()).Returns(new Game { GameStatus = GameStatus.GameOpen });

            var service = new RouletteService(
                mockRepository,
                Substitute.For<IBetRepository>(),
                Substitute.For<IGameService>(),
                Substitute.For<IBetHandlerProvider>());

            await Assert.ThrowsAsync<GameBettingOpenException>(() => service.AddBet(new AddBetRequest()));
        }

        [Fact]
        public async Task PlayGame_GetsWinningNumber()
        {
            var mockRepository = Substitute.For<IGameRepository>();
            mockRepository.GetById(Arg.Any<Guid>()).Returns(new Game { GameStatus = GameStatus.BettingClosed });
            var mockGameService = Substitute.For<IGameService>();

            var service = new RouletteService(
                mockRepository,
                Substitute.For<IBetRepository>(),
                mockGameService,
                Substitute.For<IBetHandlerProvider>());

            await service.PlayGame(Guid.NewGuid());

            mockGameService.Received().GetWinningNumber();
        }

        [Fact]
        public async Task PlayGame_ReturnsExpectedResponseWhenNoBets()
        {
            var expectedGameId = Guid.NewGuid();
            var expectedWinningNumber = 1;
            var mockRepository = Substitute.For<IGameRepository>();
            mockRepository.GetById(Arg.Any<Guid>()).Returns(new Game { GameStatus = GameStatus.BettingClosed });
            var mockGameService = Substitute.For<IGameService>();
            mockGameService.GetWinningNumber().Returns(expectedWinningNumber);

            var service = new RouletteService(
                mockRepository,
                Substitute.For<IBetRepository>(),
                mockGameService,
                Substitute.For<IBetHandlerProvider>());

            var response = await service.PlayGame(expectedGameId);

            response.GameId.Should().Be(expectedGameId);
            response.WinningNumber.Should().Be(expectedWinningNumber);
            response.WinningBets.Should().BeEmpty();
            response.CustomerTotalWinnings.Should().BeEmpty();
        }

        [Fact]
        public async Task PlayGame_GetsAllBetsForGame()
        {
            var expectedGameId = Guid.NewGuid();
            var mockGameRepository = Substitute.For<IGameRepository>();
            mockGameRepository.GetById(Arg.Any<Guid>()).Returns(new Game { GameStatus = GameStatus.BettingClosed });
            var mockGameService = Substitute.For<IGameService>();
            mockGameService.GetWinningNumber().Returns(1);
            var mockBetRepository = Substitute.For<IBetRepository>();

            var service = new RouletteService(
                mockGameRepository,
                mockBetRepository,
                mockGameService,
                Substitute.For<IBetHandlerProvider>());

            await service.PlayGame(expectedGameId);

            await mockBetRepository.Received().GetAllBetsForGame(expectedGameId);
        }

        [Fact]
        public async Task PlayGame_GetsBetHandler()
        {
            var expectedBetType = BetType.Single;
            var mockGameRepository = Substitute.For<IGameRepository>();
            mockGameRepository.GetById(Arg.Any<Guid>()).Returns(new Game { GameStatus = GameStatus.BettingClosed });
            var mockGameService = Substitute.For<IGameService>();
            mockGameService.GetWinningNumber().Returns(1);
            var mockBetRepository = Substitute.For<IBetRepository>();
            mockBetRepository.GetAllBetsForGame(Arg.Any<Guid>()).Returns(new[] { new Bet { BetType = expectedBetType } });
            var mockBetHandlerProvider = Substitute.For<IBetHandlerProvider>();

            var service = new RouletteService(
                mockGameRepository,
                mockBetRepository,
                mockGameService,
                mockBetHandlerProvider);

            await service.PlayGame(Guid.NewGuid());

            mockBetHandlerProvider.Received().GetBetHandler(expectedBetType);
        }

        [Fact]
        public async Task PlayGame_ChecksIfWinningBet()
        {
            var expectedBetPosition = 2;
            var expectedWinningNumber = 1;
            var mockGameRepository = Substitute.For<IGameRepository>();
            mockGameRepository.GetById(Arg.Any<Guid>()).Returns(new Game { GameStatus = GameStatus.BettingClosed });
            var mockGameService = Substitute.For<IGameService>();
            mockGameService.GetWinningNumber().Returns(expectedWinningNumber);
            var mockBetRepository = Substitute.For<IBetRepository>();
            mockBetRepository.GetAllBetsForGame(Arg.Any<Guid>()).Returns(new[] { new Bet { BetType = BetType.Single, Position = expectedBetPosition } });
            var mockBetHandler = Substitute.For<IBetHandler>();
            var mockBetHandlerProvider = Substitute.For<IBetHandlerProvider>();
            mockBetHandlerProvider.GetBetHandler(Arg.Any<BetType>()).Returns(mockBetHandler);

            var service = new RouletteService(
                mockGameRepository,
                mockBetRepository,
                mockGameService,
                mockBetHandlerProvider);

            await service.PlayGame(Guid.NewGuid());

            mockBetHandler.Received().IsWinningBet(expectedBetPosition, expectedWinningNumber);
        }

        [Fact]
        public async Task PlayGame_CalculatesWinnings()
        {
            var mockGameRepository = Substitute.For<IGameRepository>();
            mockGameRepository.GetById(Arg.Any<Guid>()).Returns(new Game { GameStatus = GameStatus.BettingClosed });
            var mockGameService = Substitute.For<IGameService>();
            mockGameService.GetWinningNumber().Returns(1);
            var mockBetRepository = Substitute.For<IBetRepository>();
            mockBetRepository.GetAllBetsForGame(Arg.Any<Guid>()).Returns(new[] { new Bet { BetType = BetType.Single } });
            var mockBetHandler = Substitute.For<IBetHandler>();
            var mockBetHandlerProvider = Substitute.For<IBetHandlerProvider>();
            mockBetHandlerProvider.GetBetHandler(Arg.Any<BetType>()).Returns(mockBetHandler);
            mockBetHandler.IsWinningBet(Arg.Any<int>(), Arg.Any<int>()).Returns(true);

            var service = new RouletteService(
                mockGameRepository,
                mockBetRepository,
                mockGameService,
                mockBetHandlerProvider);

            await service.PlayGame(Guid.NewGuid());

            //TODO can call with bet?
            mockBetHandler.Received().CalculateWinnings(Arg.Any<Bet>());
        }

        [Fact]
        public async Task PlayGame_ReturnsWinnings()
        {
            var expectedGameId = Guid.NewGuid();
            var expectedCustomerId = Guid.NewGuid();
            var expectedBetId = Guid.NewGuid();
            var expectedBetType = BetType.Single;
            var expectedPosition = 2;
            var expectedAmountBet = 50.0;
            var expectedAmountWon = 200.0;
            var expectedWinningNumber = 1;
            var expectedWinningBet = new WinningBet { Id = expectedBetId, BetType = expectedBetType, Position = expectedPosition, AmountBet = expectedAmountBet, AmountWon = expectedAmountWon };
            var mockGameRepository = Substitute.For<IGameRepository>();
            mockGameRepository.GetById(Arg.Any<Guid>()).Returns(new Game { GameStatus = GameStatus.BettingClosed });
            var mockGameService = Substitute.For<IGameService>();
            mockGameService.GetWinningNumber().Returns(expectedWinningNumber);
            var mockBetRepository = Substitute.For<IBetRepository>();
            mockBetRepository.GetAllBetsForGame(Arg.Any<Guid>()).Returns(new[] { new Bet { BetType = BetType.Single } });
            var mockBetHandler = Substitute.For<IBetHandler>();
            var mockBetHandlerProvider = Substitute.For<IBetHandlerProvider>();
            mockBetHandlerProvider.GetBetHandler(Arg.Any<BetType>()).Returns(mockBetHandler);
            mockBetHandler.IsWinningBet(Arg.Any<int>(), Arg.Any<int>()).Returns(true);
            mockBetHandler.CalculateWinnings(Arg.Any<Bet>()).Returns(expectedWinningBet);

            var service = new RouletteService(
                mockGameRepository,
                mockBetRepository,
                mockGameService,
                mockBetHandlerProvider);

            var response = await service.PlayGame(expectedGameId);

            response.GameId.Should().Be(expectedGameId);
            response.WinningNumber.Should().Be(expectedWinningNumber);

            response.WinningBets.Count.Should().Be(1);
            response.WinningBets.First().Key.Should().Be(expectedCustomerId.ToString());
            response.WinningBets.First().Value.Count().Should().Be(1);
            var winningBet = response.WinningBets.First().Value.First();
            winningBet.Id.Should().Be(expectedCustomerId.ToString());
            winningBet.BetType.Should().Be(expectedBetType);
            winningBet.Position.Should().Be(expectedPosition);
            winningBet.AmountBet.Should().Be(expectedAmountBet);
            winningBet.AmountWon.Should().Be(expectedAmountBet);

            response.CustomerTotalWinnings.Count.Should().Be(1);
            response.CustomerTotalWinnings.First().Key.Should().Be(expectedCustomerId.ToString());
            var totalWinnings = response.CustomerTotalWinnings.First().Value;
            totalWinnings.Should().Be(expectedAmountWon);
        }

        [Fact]
        public async Task PlayGame_SumsWinnings()
        {
            var expectedGameId = Guid.NewGuid();
            var expectedCustomerId = Guid.NewGuid();
            var expectedFirstAmountWon = 200.0;
            var expectedSecondAmountWon = 300.0;
            var expectedWinningNumber = 1;
            var expectedFirstWinningBet = new WinningBet { AmountWon = expectedFirstAmountWon };
            var expectedSecondWinningBet = new WinningBet { AmountWon = expectedSecondAmountWon };
            var mockGameRepository = Substitute.For<IGameRepository>();
            mockGameRepository.GetById(Arg.Any<Guid>()).Returns(new Game { GameStatus = GameStatus.BettingClosed });
            var mockGameService = Substitute.For<IGameService>();
            mockGameService.GetWinningNumber().Returns(expectedWinningNumber);
            var mockBetRepository = Substitute.For<IBetRepository>();
            mockBetRepository.GetAllBetsForGame(Arg.Any<Guid>()).Returns(new[] { new Bet { BetType = BetType.Single }, new Bet { BetType = BetType.Single } });
            var mockBetHandler = Substitute.For<IBetHandler>();
            var mockBetHandlerProvider = Substitute.For<IBetHandlerProvider>();
            mockBetHandlerProvider.GetBetHandler(Arg.Any<BetType>()).Returns(mockBetHandler);
            mockBetHandler.IsWinningBet(Arg.Any<int>(), Arg.Any<int>()).Returns(true);
            mockBetHandler.CalculateWinnings(Arg.Any<Bet>()).Returns(expectedFirstWinningBet, expectedSecondWinningBet);

            var service = new RouletteService(
                mockGameRepository,
                mockBetRepository,
                mockGameService,
                mockBetHandlerProvider);

            var response = await service.PlayGame(expectedGameId);

            response.GameId.Should().Be(expectedGameId);
            response.WinningNumber.Should().Be(expectedWinningNumber);
            response.CustomerTotalWinnings.First().Key.Should().Be(expectedCustomerId.ToString());
            response.CustomerTotalWinnings.First().Value.Should().Be(500.0); // 200.0 + 300.0
        }

        [Fact]
        public async Task PlayGame_ClosesGame()
        {
            var expectedGameId = Guid.NewGuid();
            var mockGameRepository = Substitute.For<IGameRepository>();
            mockGameRepository.GetById(Arg.Any<Guid>()).Returns(new Game { GameStatus = GameStatus.BettingClosed });
            var mockGameService = Substitute.For<IGameService>();
            mockGameService.GetWinningNumber().Returns(1);
            var mockBetRepository = Substitute.For<IBetRepository>();
            mockBetRepository.GetAllBetsForGame(Arg.Any<Guid>()).Returns(new[] { new Bet { BetType = BetType.Single }, new Bet { BetType = BetType.Single } });
            var mockBetHandler = Substitute.For<IBetHandler>();
            var mockBetHandlerProvider = Substitute.For<IBetHandlerProvider>();
            mockBetHandlerProvider.GetBetHandler(Arg.Any<BetType>()).Returns(mockBetHandler);
            mockBetHandler.IsWinningBet(Arg.Any<int>(), Arg.Any<int>()).Returns(true);
            mockBetHandler.CalculateWinnings(Arg.Any<Bet>()).Returns(new WinningBet());

            var service = new RouletteService(
                mockGameRepository,
                mockBetRepository,
                mockGameService,
                mockBetHandlerProvider);

            await service.PlayGame(expectedGameId);

            await mockGameRepository.Received().CloseGame(expectedGameId);
        }

        #endregion
    }
}
