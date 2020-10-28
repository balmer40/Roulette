using FluentAssertions;
using Roulette.Exceptions;
using Roulette.Repositories;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Roulette.Tests.Repositories
{
    public class GameRepositoryTest
    {
        #region CreateGame

        [Fact]
        public async Task CreateGame_GetById_CreatesNewGame()
        {
            var repository = new GameRepository();

            var expectedGameId = await repository.CreateGame();

            var result = await repository.GetById(expectedGameId);

            result.Id.Should().Be(expectedGameId);
            result.IsOpen.Should().Be(true);
            result.OpenedAt.Should().NotBe(default);
            result.ClosedAt.Should().Be(null);
        }

        #endregion

        #region CloseBets

        [Fact]
        public async Task CloseBets_ClosesBetsOnGame()
        {
            var repository = new GameRepository();

            var gameId = await repository.CreateGame();
            await repository.CloseBets(gameId);

            var result = await repository.GetById(gameId);

            result.Id.Should().Be(gameId);
            result.IsOpen.Should().Be(false);
            result.OpenedAt.Should().NotBe(default);
            result.ClosedAt.Should().NotBe(default);
        }

        [Fact]
        public async Task CloseBets_ThrowsWhenGameNotFound()
        {
            var repository = new GameRepository();

            await Assert.ThrowsAsync<GameNotFoundException>(() => repository.CloseBets(Guid.NewGuid()));
        }

        #endregion

        #region GetById

        // do not need to test GetById_ReturnsGame as this is tested in CreateGame_GetById_CreatesNewGame
        // and would be the same test

        [Fact]
        public async Task GetById_ThrowsWhenGameNotFound()
        {
            var repository = new GameRepository();

            await Assert.ThrowsAsync<GameNotFoundException>(() => repository.GetById(Guid.NewGuid()));
        }

        #endregion
    }
}
