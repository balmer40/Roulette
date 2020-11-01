using FluentAssertions;
using Roulette.Exceptions;
using Roulette.Models;
using Roulette.Repositories;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Roulette.Tests.Repositories
{
    public class BetRepositoryStubTests
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

            var expectedBet = await repository.CreateBet(
                expectedGameId,
                expectedCustomerId,
                expectedBetType,
                expectedPosition,
                expectedAmount);

            var bets = await repository.GetAllBetsForGame(expectedGameId);

            bets.Length.Should().Be(1);
            bets[0].Id.Should().Be(expectedBet.Id);
            bets[0].GameId.Should().Be(expectedGameId);
            bets[0].CustomerId.Should().Be(expectedCustomerId);
            bets[0].BetType.Should().Be(expectedBetType);
            bets[0].Position.Should().Be(expectedPosition);
            bets[0].Amount.Should().Be(expectedAmount);
        }

        [Fact]
        public async Task CreateBet_GetAllBetsForGame_ReturnsBet()
        {
            var expectedGameId = Guid.NewGuid();
            var expectedCustomerId = Guid.NewGuid();
            var expectedBetType = BetType.Single;
            var expectedPosition = 1;
            var expectedAmount = 50.0;
            var repository = new BetRepositoryStub();

            var bet = await repository.CreateBet(
                expectedGameId,
                expectedCustomerId,
                expectedBetType,
                expectedPosition,
                expectedAmount);

            bet.Id.Should().NotBeEmpty();
            bet.GameId.Should().Be(expectedGameId);
            bet.CustomerId.Should().Be(expectedCustomerId);
            bet.BetType.Should().Be(expectedBetType);
            bet.Position.Should().Be(expectedPosition);
            bet.Amount.Should().Be(expectedAmount);
        }

        [Fact]
        public async Task CreateBet_ThrowsWhenBetAlreadyExists()
        {
            var expectedGameId = Guid.NewGuid();
            var expectedCustomerId = Guid.NewGuid();
            var expectedBetType = BetType.Single;
            var expectedPosition = 1;
            var expectedAmount = 50.0;
            var repository = new BetRepositoryStub();

            await repository.CreateBet(
                expectedGameId,
                expectedCustomerId,
                expectedBetType,
                expectedPosition,
                expectedAmount);

            await Assert.ThrowsAsync<BetAlreadyExistsException>(() =>
                repository.CreateBet(expectedGameId, expectedCustomerId, expectedBetType, expectedPosition, 1));
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
        public async Task UpdateBet_ReturnsBet()
        {
            var expectedGameId = Guid.NewGuid();
            var expectedCustomerId = Guid.NewGuid();
            var expectedBetType = BetType.Single;
            var expectedPosition = 1;
            var firstAmount = 10.0;
            var secondAmount = 50.0;
            var repository = new BetRepositoryStub();

            var bet = await repository.CreateBet(
                expectedGameId,
                expectedCustomerId,
                expectedBetType,
                expectedPosition,
                firstAmount);

            var updatedBet = await repository.UpdateBet(bet.Id, secondAmount);

            updatedBet.Id.Should().Be(bet.Id);
            updatedBet.GameId.Should().Be(expectedGameId);
            updatedBet.CustomerId.Should().Be(expectedCustomerId);
            updatedBet.BetType.Should().Be(expectedBetType);
            updatedBet.Position.Should().Be(expectedPosition);
            updatedBet.Amount.Should().Be(secondAmount);
        }

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

            var bet = await repository.CreateBet(
                expectedGameId,
                expectedCustomerId,
                expectedBetType,
                expectedPosition,
                firstAmount);

            await repository.UpdateBet(bet.Id, secondAmount);

            var bets = await repository.GetAllBetsForGame(expectedGameId);

            bets.Length.Should().Be(1);
            bets[0].Id.Should().Be(bet.Id);
            bets[0].GameId.Should().Be(expectedGameId);
            bets[0].CustomerId.Should().Be(expectedCustomerId);
            bets[0].BetType.Should().Be(expectedBetType);
            bets[0].Position.Should().Be(expectedPosition);
            bets[0].Amount.Should().Be(secondAmount);
        }

        [Fact]
        public async Task UpdateBet_ThrowsWhenGameNotFound()
        {
            var repository = new BetRepositoryStub();

            await Assert.ThrowsAsync<BetNotFoundException>(() => repository.UpdateBet(Guid.NewGuid(), 1));
        }

        #endregion

        #region DeleteBet

        [Fact]
        public async Task DeleteBet_DeletesBet()
        {
            var repository = new BetRepositoryStub();

            var gameId = Guid.NewGuid();
            var bet = await repository.CreateBet(
                gameId,
                Guid.NewGuid(),
                BetType.Single,
                1,
                50.0);

            await repository.DeleteBet(bet.Id);

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
