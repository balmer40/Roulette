using FluentAssertions;
using NSubstitute;
using Roulette.Controllers;
using Roulette.Models.Requests;
using Roulette.Services;
using System;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Xunit;

namespace Roulette.Tests.Controllers
{
    public class RouletteControllerTest
    {
        [Fact]
        public async Task NewGame_CreatesNewGame()
        {
            var mockService = Substitute.For<IRouletteService>();
            var controller = new RouletteController(mockService);

            await controller.NewGame();
            await mockService.Received().CreateNewGame();
        }

        [Fact]
        public async Task NewGame_ReturnsGameId()
        {
            var expectedGameId = Guid.NewGuid();
            var mockService = Substitute.For<IRouletteService>();
            mockService.CreateNewGame().Returns(expectedGameId);
            var controller = new RouletteController(mockService);

            var result = await controller.NewGame() as OkNegotiatedContentResult<Guid>;

            result.Content.Should().Be(expectedGameId);
        }

        [Fact]
        public async Task CloseBets_ClosesBets()
        {
            var expectedRequest = new CloseBetsRequest();
            var mockService = Substitute.For<IRouletteService>();
            var controller = new RouletteController(mockService);

            await controller.CloseBets(expectedRequest);
            await mockService.Received().CloseBets(expectedRequest);
        }
    }
}
