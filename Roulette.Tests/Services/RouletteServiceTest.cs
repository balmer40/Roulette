using FluentAssertions;
using NSubstitute;
using Roulette.Models.Requests;
using Roulette.Repositories;
using Roulette.Services;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Roulette.Tests.Services
{
    public class RouletteServiceTest
    {
        [Fact]
        public async Task CreateNewGame_CreatesNewGame()
        {
            var mockRepository = Substitute.For<IGameRepository>();
            var service = new RouletteService(mockRepository);

            await service.CreateNewGame();
            await mockRepository.Received().CreateNewGame();
        }

        [Fact]
        public async Task CreateNewGame_ReturnsGameId()
        {
            var expectedGameId = Guid.NewGuid();
            var mockRepository = Substitute.For<IGameRepository>();
            mockRepository.CreateNewGame().Returns(expectedGameId);
            var service = new RouletteService(mockRepository);

            var result = await service.CreateNewGame();

            result.Should().Be(expectedGameId);
        }

        [Fact]
        public async Task CloseBets_ClosesBetsOnGame()
        {
            var expectedGameId = Guid.NewGuid();
            var mockRepository = Substitute.For<IGameRepository>();
            var service = new RouletteService(mockRepository);

            await service.CloseBets(new CloseBetsRequest { GameId = expectedGameId });

            await mockRepository.Received().CloseBets(expectedGameId);
        }
    }
}
