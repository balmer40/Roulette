using FluentAssertions;
using Roulette.Repositories;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Roulette.Tests.Repositories
{
    public class GameRepositoryTest
    {
        [Fact]
        public async Task CreateNewGame_CreatesNewGameAndReturnsGame()
        {
            var repository = new GameRepository();

            var expectedGameId = await repository.CreateNewGame();

            var result = await repository.GetById(expectedGameId);

            result.GameId.Should().Be(expectedGameId);
            result.IsOpen.Should().Be(true);
            result.OpenedAt.Should().NotBe(default);
            result.ClosedAt.Should().Be(null);
        }

        [Fact]
        public async Task CloseBets_ClosesBetsOnGame()
        {
            var repository = new GameRepository();

            var gameId = await repository.CreateNewGame();
            await repository.CloseBets(gameId);

            var result = await repository.GetById(gameId);

            result.GameId.Should().Be(gameId);
            result.IsOpen.Should().Be(false);
            result.OpenedAt.Should().NotBe(default);
            result.ClosedAt.Should().NotBe(default);
        }
    }
}
