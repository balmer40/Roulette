using FluentAssertions;
using Roulette.Repositories;
using System.Threading.Tasks;
using Xunit;

namespace Roulette.Tests.Repositories
{
    public class GameRepositoryTest
    {
        [Fact]
        public async Task CreateNewGame_GetById_CreatesNewGameAndReturnsGame()
        {
            var repository = new GameRepository();

            var expectedGameId = await repository.CreateNewGame();

            var result = await repository.GetById(expectedGameId);

            result.GameId.Should().Be(expectedGameId);
        }
    }
}
