using FluentAssertions;
using Roulette.Exceptions;
using Roulette.Models;
using Roulette.Repositories;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Roulette.Tests.Repositories
{
    public class BetRepositoryStubTest
    {
        #region CreateBet and GetAllBetsForGame

        [Fact]
        public async Task CreateBet_GetAllBetsForGame_CreatesBet()
        {
            var expectedGameId = Guid.NewGuid();
            var expectedCustomerId = Guid.NewGuid();
            var expectedBetType = BetType.Single;
            var expectedPosition = 1;
            var expectedAmount = 50.0;
            var repository = new BetRepositoryStub();

            var expectedBetId = await repository.CreateBet(
                expectedGameId,
                expectedCustomerId,
                expectedBetType,
                expectedPosition,
                expectedAmount);

            var bets = await repository.GetAllBetsForGame(expectedGameId);

            bets.Length.Should().Be(1);
            bets[0].Id.Should().Be(expectedBetId);
            bets[0].GameId.Should().Be(expectedGameId);
            bets[0].CustomerId.Should().Be(expectedCustomerId);
            bets[0].BetType.Should().Be(expectedBetType);
            bets[0].Position.Should().Be(expectedPosition);
            bets[0].Amount.Should().Be(expectedAmount);
        }

        [Fact]
        public async Task CreateBet_GetAllBetsForGame_ReturnsBetId()
        {
            var repository = new BetRepositoryStub();

            var betId = await repository.CreateBet(
                Guid.NewGuid(),
                Guid.NewGuid(),
                BetType.Single,
                1,
                50.0);

            betId.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetAllBetsForGame_ReturnsEmptyArrayWhenNoBetsForGameId()
        {
            var repository = new BetRepositoryStub();

            var bets = await repository.GetAllBetsForGame(Guid.NewGuid());

            bets.Should().BeEmpty();
        }

        #endregion

        #region UpdateBet

        [Fact]
        public async Task UpdateBet_UpdatesBet()
        {
            var repository = new BetRepositoryStub();

            var expectedGameId = Guid.NewGuid();
            var expectedCustomerId = Guid.NewGuid();
            var expectedBetType = BetType.Single;
            var expectedPosition = 1;
            var firstAmount = 10.0;
            var secondAmount = 50.0;

            var expectedBetId = await repository.CreateBet(
                expectedGameId,
                expectedCustomerId,
                expectedBetType,
                expectedPosition,
                firstAmount);

            await repository.UpdateBet(expectedBetId, secondAmount);

            var bets = await repository.GetAllBetsForGame(expectedGameId);

            bets.Length.Should().Be(1);
            bets[0].Id.Should().Be(expectedBetId);
            bets[0].GameId.Should().Be(expectedGameId);
            bets[0].CustomerId.Should().Be(expectedCustomerId);
            bets[0].BetType.Should().Be(expectedBetType);
            bets[0].Position.Should().Be(expectedPosition);
            bets[0].Amount.Should().Be(firstAmount + secondAmount);
        }

        [Fact]
        public async Task UpdateBet_ThrowsWhenGameNotFound()
        {
            var repository = new BetRepositoryStub();

            await Assert.ThrowsAsync<BetNotFoundException>(() => repository.UpdateBet(Guid.NewGuid(), 1));
        }

        [Fact]
        public async Task UpdateBet_ThrowsWhenAmountTooHigh()
        {
            var repository = new BetRepositoryStub();

            var betId = await repository.CreateBet(
                Guid.NewGuid(),
                Guid.NewGuid(),
                BetType.Single,
                1,
                9999.0);

            await Assert.ThrowsAsync<UpdateAmountTooHighException>(() => repository.UpdateBet(betId, 5));
        }

        #endregion

        #region DeleteBet

        [Fact]
        public async Task DeleteBet_DeletesBet()
        {
            var repository = new BetRepositoryStub();

            var gameId = Guid.NewGuid();
            var betId = await repository.CreateBet(
                gameId,
                Guid.NewGuid(),
                BetType.Single,
                1,
                50.0);

            await repository.DeleteBet(betId);

            var bets = await repository.GetAllBetsForGame(gameId);
            bets.Length.Should().Be(0);
        }

        [Fact]
        public async Task DeleteBet_ThrowsWhenBetNotFound()
        {
            var repository = new BetRepositoryStub();

            await Assert.ThrowsAsync<BetNotFoundException>(() => repository.DeleteBet(Guid.NewGuid()));
        }

        #endregion
    }
}
