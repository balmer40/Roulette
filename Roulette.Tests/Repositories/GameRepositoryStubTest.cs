using FluentAssertions;
using Roulette.Exceptions;
using Roulette.Models;
using Roulette.Repositories;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Roulette.Tests.Repositories
{
    public class GameRepositoryStubTest
    {
        #region CreateGame

        [Fact]
        public async Task CreateGame_GetById_CreatesNewGame()
        {
            var repository = new GameRepositoryStub();

            var expectedGameId = await repository.CreateGame();

            var result = await repository.GetById(expectedGameId);

            result.Id.Should().Be(expectedGameId);
            result.GameStatus.Should().Be(GameStatus.GameOpen);
            result.OpenedAt.Should().NotBe(default);
            result.ClosedAt.Should().Be(null);
        }

        [Fact]
        public async Task CreateGame_ReturnsGameId()
        {
            var repository = new GameRepositoryStub();

            var gameId = await repository.CreateGame();

            gameId.Should().NotBeEmpty();
        }

        #endregion

        #region CloseBetting

        [Fact]
        public async Task CloseBetting_ClosesBettingOnGame()
        {
            var repository = new GameRepositoryStub();

            var expectedGameId = await repository.CreateGame();
            await repository.CloseBetting(expectedGameId);

            var result = await repository.GetById(expectedGameId);

            result.Id.Should().Be(expectedGameId);
            result.GameStatus.Should().Be(GameStatus.BettingClosed);
            result.OpenedAt.Should().NotBe(default);
            result.ClosedAt.Should().Be(default);
        }

        [Fact]
        public async Task CloseBetting_ThrowsWhenGameNotFound()
        {
            var repository = new GameRepositoryStub();

            await Assert.ThrowsAsync<GameNotFoundException>(() => repository.CloseBetting(Guid.NewGuid()));
        }

        #endregion

        #region CloseGame

        [Fact]
        public async Task CloseGame_ClosesGame()
        {
            var repository = new GameRepositoryStub();

            var expectedGameId = await repository.CreateGame();
            await repository.CloseGame(expectedGameId);

            var result = await repository.GetById(expectedGameId);

            result.Id.Should().Be(expectedGameId);
            result.GameStatus.Should().Be(GameStatus.GameClosed);
            result.OpenedAt.Should().NotBe(default);
            result.ClosedAt.Should().NotBe(default);
        }

        [Fact]
        public async Task CloseGame_ThrowsWhenGameNotFound()
        {
            var repository = new GameRepositoryStub();

            await Assert.ThrowsAsync<GameNotFoundException>(() => repository.CloseGame(Guid.NewGuid()));
        }

        #endregion

        #region GetById

        // do not need to test GetById_ReturnsGame as this is tested in CreateGame_GetById_CreatesNewGame
        // and would be the same test

        [Fact]
        public async Task GetById_ThrowsWhenGameNotFound()
        {
            var repository = new GameRepositoryStub();

            await Assert.ThrowsAsync<GameNotFoundException>(() => repository.GetById(Guid.NewGuid()));
        }

        #endregion
    }
}
