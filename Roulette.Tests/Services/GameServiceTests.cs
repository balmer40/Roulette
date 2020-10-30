using FluentAssertions;
using NSubstitute;
using Roulette.Exceptions;
using Roulette.Handlers;
using Roulette.Models;
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
    public class GameServiceTests
    {
        #region CreateNewGame

        [Fact]
        public async Task CreateNewGame_CreatesNewGame()
        {
            var mockRepository = Substitute.For<IGameRepository>();
            var service = new GameService(
                mockRepository,
                Substitute.For<IBetRepository>(),
                Substitute.For<ISpinWheelService>(),
                Substitute.For<IBetTypeHandlerProvider>());

            await service.CreateNewGame();
            await mockRepository.Received().CreateGame();
        }

        [Fact]
        public async Task CreateNewGame_ReturnsNewGameResponse()
        {
            var expectedGameId = Guid.NewGuid();
            var mockRepository = Substitute.For<IGameRepository>();
            mockRepository.CreateGame().Returns(expectedGameId);
            var service = new GameService(
                mockRepository,
                Substitute.For<IBetRepository>(),
                Substitute.For<ISpinWheelService>(),
                Substitute.For<IBetTypeHandlerProvider>());

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
            mockRepository.GetById(Arg.Any<Guid>()).Returns(new Game());

            var service = new GameService(
                mockRepository,
                Substitute.For<IBetRepository>(),
                Substitute.For<ISpinWheelService>(),
                Substitute.For<IBetTypeHandlerProvider>());

            await service.CloseBetting(expectedGameId);

            await mockRepository.Received().CloseBetting(expectedGameId);
        }

        [Fact]
        public async Task CloseBetting_ThrowsWhenGameIsNotOpen()
        {
            var expectedGameId = Guid.NewGuid();
            var mockRepository = Substitute.For<IGameRepository>();
            mockRepository.GetById(Arg.Any<Guid>()).Returns(new Game { GameStatus = GameStatus.GameClosed });
            var service = new GameService(
                mockRepository,
                Substitute.For<IBetRepository>(),
                Substitute.For<ISpinWheelService>(),
                Substitute.For<IBetTypeHandlerProvider>());

            await Assert.ThrowsAsync<GameClosedException>(() => service.CloseBetting(expectedGameId));
        }

        #endregion

        #region PlayGame

        [Fact]
        public async Task PlayGame_ThrowsWhenGameClosed()
        {
            var mockRepository = Substitute.For<IGameRepository>();
            mockRepository.GetById(Arg.Any<Guid>()).Returns(new Game { GameStatus = GameStatus.GameClosed });

            var service = new GameService(
                mockRepository,
                Substitute.For<IBetRepository>(),
                Substitute.For<ISpinWheelService>(),
                Substitute.For<IBetTypeHandlerProvider>());

            await Assert.ThrowsAsync<GameClosedException>(() => service.PlayGame(Guid.NewGuid()));
        }

        [Fact]
        public async Task PlayGame_ThrowsWhenBetsStillOpen()
        {
            var mockRepository = Substitute.For<IGameRepository>();
            mockRepository.GetById(Arg.Any<Guid>()).Returns(new Game { GameStatus = GameStatus.GameOpen });

            var service = new GameService(
                mockRepository,
                Substitute.For<IBetRepository>(),
                Substitute.For<ISpinWheelService>(),
                Substitute.For<IBetTypeHandlerProvider>());

            await Assert.ThrowsAsync<GameBettingOpenException>(() => service.PlayGame(Guid.NewGuid()));
        }

        [Fact]
        public async Task PlayGame_GetsWinningNumber()
        {
            var mockRepository = Substitute.For<IGameRepository>();
            mockRepository.GetById(Arg.Any<Guid>()).Returns(new Game { GameStatus = GameStatus.BettingClosed });
            var mockGameService = Substitute.For<ISpinWheelService>();

            var service = new GameService(
                mockRepository,
                Substitute.For<IBetRepository>(),
                mockGameService,
                Substitute.For<IBetTypeHandlerProvider>());

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
            var mockGameService = Substitute.For<ISpinWheelService>();
            mockGameService.GetWinningNumber().Returns(expectedWinningNumber);

            var service = new GameService(
                mockRepository,
                Substitute.For<IBetRepository>(),
                mockGameService,
                Substitute.For<IBetTypeHandlerProvider>());

            var response = await service.PlayGame(expectedGameId);

            response.GameId.Should().Be(expectedGameId);
            response.WinningNumber.Should().Be(expectedWinningNumber);
            response.WinningBets.Should().BeEmpty();
            response.LosingBets.Should().BeEmpty();
            response.CustomerTotalWinnings.Should().BeEmpty();
        }

        [Fact]
        public async Task PlayGame_GetsAllBetsForGame()
        {
            var expectedGameId = Guid.NewGuid();
            var mockGameRepository = Substitute.For<IGameRepository>();
            mockGameRepository.GetById(Arg.Any<Guid>()).Returns(new Game { GameStatus = GameStatus.BettingClosed });
            var mockGameService = Substitute.For<ISpinWheelService>();
            mockGameService.GetWinningNumber().Returns(1);
            var mockBetRepository = Substitute.For<IBetRepository>();

            var service = new GameService(
                mockGameRepository,
                mockBetRepository,
                mockGameService,
                Substitute.For<IBetTypeHandlerProvider>());

            await service.PlayGame(expectedGameId);

            await mockBetRepository.Received().GetAllBetsForGame(expectedGameId);
        }

        [Fact]
        public async Task PlayGame_GetsBetTypeHandler()
        {
            var expectedBetType = BetType.Single;
            var mockGameRepository = Substitute.For<IGameRepository>();
            mockGameRepository.GetById(Arg.Any<Guid>()).Returns(new Game { GameStatus = GameStatus.BettingClosed });
            var mockGameService = Substitute.For<ISpinWheelService>();
            mockGameService.GetWinningNumber().Returns(1);
            var mockBetRepository = Substitute.For<IBetRepository>();
            mockBetRepository.GetAllBetsForGame(Arg.Any<Guid>()).Returns(new[] { new Bet { BetType = expectedBetType } });
            var mockBetTypeHandlerProvider = Substitute.For<IBetTypeHandlerProvider>();

            var service = new GameService(
                mockGameRepository,
                mockBetRepository,
                mockGameService,
                mockBetTypeHandlerProvider);

            await service.PlayGame(Guid.NewGuid());

            mockBetTypeHandlerProvider.Received().GetBetTypeHandler(expectedBetType);
        }

        [Fact]
        public async Task PlayGame_ChecksIfWinningBet()
        {
            var expectedBetPosition = 2;
            var expectedWinningNumber = 1;
            var mockGameRepository = Substitute.For<IGameRepository>();
            mockGameRepository.GetById(Arg.Any<Guid>()).Returns(new Game { GameStatus = GameStatus.BettingClosed });
            var mockGameService = Substitute.For<ISpinWheelService>();
            mockGameService.GetWinningNumber().Returns(expectedWinningNumber);
            var mockBetRepository = Substitute.For<IBetRepository>();
            mockBetRepository.GetAllBetsForGame(Arg.Any<Guid>()).Returns(new[] { new Bet { BetType = BetType.Single, Position = expectedBetPosition } });
            var mockBetTypeHandler = Substitute.For<IBetTypeHandler>();
            var mockBetTypeHandlerProvider = Substitute.For<IBetTypeHandlerProvider>();
            mockBetTypeHandlerProvider.GetBetTypeHandler(Arg.Any<BetType>()).Returns(mockBetTypeHandler);

            var service = new GameService(
                mockGameRepository,
                mockBetRepository,
                mockGameService,
                mockBetTypeHandlerProvider);

            await service.PlayGame(Guid.NewGuid());

            mockBetTypeHandler.Received().IsWinningBet(expectedBetPosition, expectedWinningNumber);
        }

        [Fact]
        public async Task PlayGame_CalculatesWinnings()
        {
            var expectedBet = new Bet { BetType = BetType.Single };
            var mockGameRepository = Substitute.For<IGameRepository>();
            mockGameRepository.GetById(Arg.Any<Guid>()).Returns(new Game { GameStatus = GameStatus.BettingClosed });
            var mockGameService = Substitute.For<ISpinWheelService>();
            mockGameService.GetWinningNumber().Returns(1);
            var mockBetRepository = Substitute.For<IBetRepository>();
            mockBetRepository.GetAllBetsForGame(Arg.Any<Guid>()).Returns(new[] { expectedBet });
            var mockBetTypeHandler = Substitute.For<IBetTypeHandler>();
            var mockBetTypeHandlerProvider = Substitute.For<IBetTypeHandlerProvider>();
            mockBetTypeHandlerProvider.GetBetTypeHandler(Arg.Any<BetType>()).Returns(mockBetTypeHandler);
            mockBetTypeHandler.IsWinningBet(Arg.Any<int>(), Arg.Any<int>()).Returns(true);
            mockBetTypeHandler.CalculateWinnings(Arg.Any<Bet>()).Returns(new WinningBet());

            var service = new GameService(
                mockGameRepository,
                mockBetRepository,
                mockGameService,
                mockBetTypeHandlerProvider);

            await service.PlayGame(Guid.NewGuid());

            mockBetTypeHandler.Received().CalculateWinnings(expectedBet);
        }

        [Fact]
        public async Task PlayGame_ClosesGame()
        {
            var expectedGameId = Guid.NewGuid();
            var mockGameRepository = Substitute.For<IGameRepository>();
            mockGameRepository.GetById(Arg.Any<Guid>()).Returns(new Game { GameStatus = GameStatus.BettingClosed });
            var mockGameService = Substitute.For<ISpinWheelService>();
            mockGameService.GetWinningNumber().Returns(1);
            var mockBetRepository = Substitute.For<IBetRepository>();
            mockBetRepository.GetAllBetsForGame(Arg.Any<Guid>()).Returns(new[] { new Bet { BetType = BetType.Single }, new Bet { BetType = BetType.Single } });
            var mockBetTypeHandler = Substitute.For<IBetTypeHandler>();
            var mockBetTypeHandlerProvider = Substitute.For<IBetTypeHandlerProvider>();
            mockBetTypeHandlerProvider.GetBetTypeHandler(Arg.Any<BetType>()).Returns(mockBetTypeHandler);
            mockBetTypeHandler.IsWinningBet(Arg.Any<int>(), Arg.Any<int>()).Returns(true);
            mockBetTypeHandler.CalculateWinnings(Arg.Any<Bet>()).Returns(new WinningBet());

            var service = new GameService(
                mockGameRepository,
                mockBetRepository,
                mockGameService,
                mockBetTypeHandlerProvider);

            await service.PlayGame(expectedGameId);

            await mockGameRepository.Received().CloseGame(expectedGameId);
        }

        [Fact]
        public async Task PlayGame_ReturnsExpectedWinnings()
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
            var mockGameService = Substitute.For<ISpinWheelService>();
            mockGameService.GetWinningNumber().Returns(expectedWinningNumber);
            var mockBetRepository = Substitute.For<IBetRepository>();
            mockBetRepository.GetAllBetsForGame(Arg.Any<Guid>()).Returns(new[] { new Bet { CustomerId = expectedCustomerId, BetType = BetType.Single } });
            var mockBetTypeHandler = Substitute.For<IBetTypeHandler>();
            var mockBetTypeHandlerProvider = Substitute.For<IBetTypeHandlerProvider>();
            mockBetTypeHandlerProvider.GetBetTypeHandler(Arg.Any<BetType>()).Returns(mockBetTypeHandler);
            mockBetTypeHandler.IsWinningBet(Arg.Any<int>(), Arg.Any<int>()).Returns(true);
            mockBetTypeHandler.CalculateWinnings(Arg.Any<Bet>()).Returns(expectedWinningBet);

            var service = new GameService(
                mockGameRepository,
                mockBetRepository,
                mockGameService,
                mockBetTypeHandlerProvider);

            var response = await service.PlayGame(expectedGameId);

            response.GameId.Should().Be(expectedGameId);
            response.WinningNumber.Should().Be(expectedWinningNumber);

            response.WinningBets.Count.Should().Be(1);
            response.WinningBets.First().Key.Should().Be(expectedCustomerId.ToString());
            response.WinningBets.First().Value.Count().Should().Be(1);
            var winningBet = response.WinningBets.First().Value.First();
            winningBet.Id.Should().Be(expectedBetId.ToString());
            winningBet.BetType.Should().Be(expectedBetType);
            winningBet.Position.Should().Be(expectedPosition);
            winningBet.AmountBet.Should().Be(expectedAmountBet);
            winningBet.AmountWon.Should().Be(expectedAmountWon);

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
            var mockGameService = Substitute.For<ISpinWheelService>();
            mockGameService.GetWinningNumber().Returns(expectedWinningNumber);
            var mockBetRepository = Substitute.For<IBetRepository>();
            mockBetRepository.GetAllBetsForGame(Arg.Any<Guid>()).Returns(new[] { new Bet { CustomerId = expectedCustomerId, BetType = BetType.Single }, new Bet { CustomerId = expectedCustomerId, BetType = BetType.Single } });
            var mockBetTypeHandler = Substitute.For<IBetTypeHandler>();
            var mockBetTypeHandlerProvider = Substitute.For<IBetTypeHandlerProvider>();
            mockBetTypeHandlerProvider.GetBetTypeHandler(Arg.Any<BetType>()).Returns(mockBetTypeHandler);
            mockBetTypeHandler.IsWinningBet(Arg.Any<int>(), Arg.Any<int>()).Returns(true);
            mockBetTypeHandler.CalculateWinnings(Arg.Any<Bet>()).Returns(expectedFirstWinningBet, expectedSecondWinningBet);

            var service = new GameService(
                mockGameRepository,
                mockBetRepository,
                mockGameService,
                mockBetTypeHandlerProvider);

            var response = await service.PlayGame(expectedGameId);

            response.GameId.Should().Be(expectedGameId);
            response.WinningNumber.Should().Be(expectedWinningNumber);
            response.CustomerTotalWinnings.First().Key.Should().Be(expectedCustomerId.ToString());
            response.CustomerTotalWinnings.First().Value.Should().Be(500.0); // 200.0 + 300.0
        }

        [Fact]
        public async Task PlayGame_ReturnsExpectedLosses()
        {
            var expectedGameId = Guid.NewGuid();
            var expectedCustomerId = Guid.NewGuid();
            var expectedBetId = Guid.NewGuid();
            var expectedBetType = BetType.Single;
            var expectedPosition = 2;
            var expectedAmountBet = 50.0;
            var expectedWinningNumber = 1;
            var mockGameRepository = Substitute.For<IGameRepository>();
            mockGameRepository.GetById(Arg.Any<Guid>()).Returns(new Game { GameStatus = GameStatus.BettingClosed });
            var mockGameService = Substitute.For<ISpinWheelService>();
            mockGameService.GetWinningNumber().Returns(expectedWinningNumber);
            var mockBetRepository = Substitute.For<IBetRepository>();
            mockBetRepository.GetAllBetsForGame(Arg.Any<Guid>()).Returns(new[] { new Bet { CustomerId = expectedCustomerId, Id = expectedBetId, BetType = BetType.Single, Position = expectedPosition, Amount = expectedAmountBet} });
            var mockBetTypeHandler = Substitute.For<IBetTypeHandler>();
            var mockBetTypeHandlerProvider = Substitute.For<IBetTypeHandlerProvider>();
            mockBetTypeHandlerProvider.GetBetTypeHandler(Arg.Any<BetType>()).Returns(mockBetTypeHandler);
            mockBetTypeHandler.IsWinningBet(Arg.Any<int>(), Arg.Any<int>()).Returns(false);

            var service = new GameService(
                mockGameRepository,
                mockBetRepository,
                mockGameService,
                mockBetTypeHandlerProvider);

            var response = await service.PlayGame(expectedGameId);

            response.GameId.Should().Be(expectedGameId);
            response.WinningNumber.Should().Be(expectedWinningNumber);

            response.LosingBets.Count.Should().Be(1);
            response.LosingBets.First().Key.Should().Be(expectedCustomerId.ToString());
            response.LosingBets.First().Value.Count().Should().Be(1);
            var losingBet = response.LosingBets.First().Value.First();
            losingBet.Id.Should().Be(expectedBetId.ToString());
            losingBet.BetType.Should().Be(expectedBetType);
            losingBet.Position.Should().Be(expectedPosition);
            losingBet.AmountBet.Should().Be(expectedAmountBet);
        }

        #endregion
    }
}
