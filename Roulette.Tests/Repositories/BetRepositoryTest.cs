using FluentAssertions;
using Roulette.Exceptions;
using Roulette.Models;
using Roulette.Repositories;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Roulette.Tests.Repositories
{
    public class BetRepositoryTest
    {
        #region CreateBet

        [Fact]
        public async Task CreateBet_GetById_CreatesBet()
        {
            var expectedGameId = Guid.NewGuid();
            var expectedCustomerId = Guid.NewGuid();
            var expectedBetType = BetType.Single;
            var expectedPosition = 1;
            var expectedAmount = 50.0;
            var repository = new BetRepository();

            var expectedBetId = await repository.CreateBet(
                expectedGameId,
                expectedCustomerId,
                expectedBetType,
                expectedPosition,
                expectedAmount);

            var result = await repository.GetById(expectedBetId);

            result.Id.Should().Be(expectedBetId);
            result.GameId.Should().Be(expectedGameId);
            result.CustomerId.Should().Be(expectedCustomerId);
            result.BetType.Should().Be(expectedBetType);
            result.Position.Should().Be(expectedPosition);
            result.Amount.Should().Be(expectedAmount);
        }

        #endregion

        #region DeleteBet

        [Fact]
        public async Task DeleteBet_DeletesBet()
        {
            var repository = new BetRepository();

            var betId = await repository.CreateBet(
                Guid.NewGuid(),
                Guid.NewGuid(),
                BetType.Single,
                1,
                50.0);

            await repository.DeleteBet(betId);

            await Assert.ThrowsAsync<BetNotFoundException>(() => repository.GetById(betId));
 
        }

        [Fact]
        public async Task DeleteBet_ThrowsWhenBetNotFound()
        {
            var repository = new BetRepository();

            await Assert.ThrowsAsync<BetNotFoundException>(() => repository.DeleteBet(Guid.NewGuid()));
        }

        //TODO can we test the modify exceptions?

        #endregion

        #region GetById

        // do not need to test GetById_ReturnsBet as this is tested in CreateBet_GetById_CreatesBet
        // and would be the same test

        [Fact]
        public async Task GetById_ThrowsWhenBetNotFound()
        {
            var repository = new BetRepository();

            await Assert.ThrowsAsync<BetNotFoundException>(() => repository.GetById(Guid.NewGuid()));
        }

        #endregion
    }
}
