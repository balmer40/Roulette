using FluentAssertions;
using NSubstitute;
using Roulette.Exceptions;
using Roulette.Models;
using Roulette.Models.Requests;
using Roulette.Models.Responses;
using Roulette.Repositories;
using Roulette.Services;
using System;
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
            var service = new RouletteService(mockRepository, Substitute.For<IBetRepository>());

            await service.CreateNewGame();
            await mockRepository.Received().CreateGame();
        }

        [Fact]
        public async Task CreateNewGame_ReturnsNewGameResponse()
        {
            var expectedGameId = Guid.NewGuid();
            var mockRepository = Substitute.For<IGameRepository>();
            mockRepository.CreateGame().Returns(expectedGameId);
            var service = new RouletteService(mockRepository, Substitute.For<IBetRepository>());

            var result = await service.CreateNewGame();

            result.Should().BeOfType(typeof(NewGameResponse));
            result.GameId.Should().Be(expectedGameId);
        }

        #endregion

        #region CloseBets

        [Fact]
        public async Task CloseBets_ClosesBetsOnGame()
        {
            var expectedGameId = Guid.NewGuid();
            var mockRepository = Substitute.For<IGameRepository>();
            var service = new RouletteService(mockRepository, Substitute.For<IBetRepository>());

            await service.CloseBets(expectedGameId);

            await mockRepository.Received().CloseBets(expectedGameId);
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
            mockGameRepository.GetById(Arg.Any<Guid>()).Returns(new Game { IsOpen = true });
            var mockBetRepository = Substitute.For<IBetRepository>();
            var service = new RouletteService(mockGameRepository, mockBetRepository);

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
        public async Task AddBet_ThrowsWhenGameNotOpen()
        {
            var mockRepository = Substitute.For<IGameRepository>();
            mockRepository.GetById(Arg.Any<Guid>()).Returns(new Game { IsOpen = false });

            var service = new RouletteService(mockRepository, Substitute.For<IBetRepository>());

            await Assert.ThrowsAsync<GameClosedException>(() => service.AddBet(new AddBetRequest()));
        }

        [Fact]
        public async Task AddBet_ReturnsBetId()
        {
            var expectedBetId = Guid.NewGuid();
            var mockGameRepository = Substitute.For<IGameRepository>();
            mockGameRepository.GetById(Arg.Any<Guid>()).Returns(new Game { IsOpen = true });
            var mockBetRepository = Substitute.For<IBetRepository>();
            mockBetRepository.CreateBet(Arg.Any<Guid>(), Arg.Any<Guid>(), Arg.Any<BetType>(), Arg.Any<int>(), Arg.Any<double>())
                .Returns(expectedBetId);

            var service = new RouletteService(mockGameRepository, mockBetRepository);

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
            var mockRepository = Substitute.For<IBetRepository>();
            var service = new RouletteService(Substitute.For<IGameRepository>(), mockRepository);

            await service.DeleteBet(expectedBetId);

            await mockRepository.Received().DeleteBet(expectedBetId);
        }

        #endregion
    }
}
