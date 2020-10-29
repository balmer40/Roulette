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
    public class BetServiceTest
    {
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
            var service = new BetService(
                mockGameRepository,
                mockBetRepository);

            await service.AddBet(
                expectedGameId,
                new AddBetRequest
                {
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

            var service = new BetService(
                mockRepository,
                Substitute.For<IBetRepository>());

            await Assert.ThrowsAsync<GameClosedException>(() => service.AddBet(Guid.NewGuid(), new AddBetRequest()));
        }

        [Fact]
        public async Task AddBet_ThrowsWhenBettingIsClosed()
        {
            var mockRepository = Substitute.For<IGameRepository>();
            mockRepository.GetById(Arg.Any<Guid>()).Returns(new Game { GameStatus = GameStatus.BettingClosed });

            var service = new BetService(
                mockRepository,
                Substitute.For<IBetRepository>());

            await Assert.ThrowsAsync<GameBettingClosedException>(() => service.AddBet(Guid.NewGuid(), new AddBetRequest()));
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

            var service = new BetService(
                mockGameRepository,
                mockBetRepository);

            var result = await service.AddBet(Guid.NewGuid(), new AddBetRequest());

            result.Should().BeOfType(typeof(AddBetResponse));
            result.BetId.Should().Be(expectedBetId);
        }

        #endregion

        #region UpdateBet

        [Fact]
        public async Task UpdateBet_UpdatesBet()
        {
            var expectedBetId = Guid.NewGuid();
            var expectedAmount = 4;
            var mockGameRepository = Substitute.For<IGameRepository>();
            mockGameRepository.GetById(Arg.Any<Guid>()).Returns(new Game());
            var mockBetRepository = Substitute.For<IBetRepository>();
            var service = new BetService(
                mockGameRepository,
                mockBetRepository);

            await service.UpdateBet(Guid.NewGuid(), expectedBetId, new UpdateBetRequest { Amount = expectedAmount });

            await mockBetRepository.Received().UpdateBet(expectedBetId, expectedAmount);
        }

        [Fact]
        public async Task UpdateBet_ThrowsWhenGameIsClosed()
        {
            var mockRepository = Substitute.For<IGameRepository>();
            mockRepository.GetById(Arg.Any<Guid>()).Returns(new Game { GameStatus = GameStatus.GameClosed });

            var service = new BetService(
                mockRepository,
                Substitute.For<IBetRepository>());

            await Assert.ThrowsAsync<GameClosedException>(() => service.UpdateBet(Guid.NewGuid(), Guid.NewGuid(), new UpdateBetRequest()));
        }

        [Fact]
        public async Task UpdateBet_ThrowsWhenBettingIsClosed()
        {
            var mockRepository = Substitute.For<IGameRepository>();
            mockRepository.GetById(Arg.Any<Guid>()).Returns(new Game { GameStatus = GameStatus.BettingClosed });

            var service = new BetService(
                mockRepository,
                Substitute.For<IBetRepository>());

            await Assert.ThrowsAsync<GameBettingClosedException>(() => service.UpdateBet(Guid.NewGuid(), Guid.NewGuid(), new UpdateBetRequest()));
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
            var service = new BetService(
                mockGameRepository,
                mockBetRepository);

            await service.DeleteBet(Guid.NewGuid(), expectedBetId);

            await mockBetRepository.Received().DeleteBet(expectedBetId);
        }

        [Fact]
        public async Task DeleteBet_ThrowsWhenGameIsClosed()
        {
            var mockRepository = Substitute.For<IGameRepository>();
            mockRepository.GetById(Arg.Any<Guid>()).Returns(new Game { GameStatus = GameStatus.GameClosed });

            var service = new BetService(
                mockRepository,
                Substitute.For<IBetRepository>());

            await Assert.ThrowsAsync<GameClosedException>(() => service.DeleteBet(Guid.NewGuid(), Guid.NewGuid()));
        }

        [Fact]
        public async Task DeleteBet_ThrowsWhenBettingIsClosed()
        {
            var mockRepository = Substitute.For<IGameRepository>();
            mockRepository.GetById(Arg.Any<Guid>()).Returns(new Game { GameStatus = GameStatus.BettingClosed });

            var service = new BetService(
                mockRepository,
                Substitute.For<IBetRepository>());

            await Assert.ThrowsAsync<GameBettingClosedException>(() => service.DeleteBet(Guid.NewGuid(), Guid.NewGuid()));
        }

        #endregion
    }
}
