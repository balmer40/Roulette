using FluentAssertions;
using NSubstitute;
using Roulette.Controllers;
using Roulette.Models.Requests;
using Roulette.Models.Responses;
using Roulette.Services;
using System;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Xunit;

namespace Roulette.Tests.Controllers
{
    public class RouletteControllerTest
    {
        #region NewGame

        [Fact]
        public async Task NewGame_CreatesNewGame()
        {
            var mockService = Substitute.For<IRouletteService>();
            var controller = new RouletteController(mockService);

            await controller.NewGame();
            await mockService.Received().CreateNewGame();
        }

        [Fact]
        public async Task NewGame_ReturnsGameResponse()
        {
            var expectedNewGameResponse = new NewGameResponse();
            var mockService = Substitute.For<IRouletteService>();
            mockService.CreateNewGame().Returns(expectedNewGameResponse);
            var controller = new RouletteController(mockService);

            var result = await controller.NewGame() as OkNegotiatedContentResult<NewGameResponse>;
            result.Content.Should().Be(expectedNewGameResponse);
        }

        #endregion

        #region CloseBetting

        [Fact]
        public async Task CloseBetting_ClosesBetting()
        {
            var expectedGameId = Guid.NewGuid();
            var mockService = Substitute.For<IRouletteService>();
            var controller = new RouletteController(mockService);

            await controller.CloseBetting(expectedGameId);
            await mockService.Received().CloseBetting(expectedGameId);
        }

        #endregion

        #region PlayGame

        [Fact]
        public async Task PlayGame_PlaysGame()
        {
            var expectedGameId = Guid.NewGuid();
            var mockService = Substitute.For<IRouletteService>();
            var controller = new RouletteController(mockService);

            await controller.PlayGame(expectedGameId);
            await mockService.Received().PlayGame(expectedGameId);
        }

        [Fact]
        public async Task PlayGame_ReturnsPlayResponse()
        {
            var expectedPlayResponse = new PlayGameResponse();
            var mockService = Substitute.For<IRouletteService>();
            mockService.PlayGame(Arg.Any<Guid>()).Returns(expectedPlayResponse);
            var controller = new RouletteController(mockService);

            var result = await controller.PlayGame(Guid.NewGuid()) as OkNegotiatedContentResult<NewGameResponse>;
            result.Content.Should().Be(expectedPlayResponse);
        }

        #endregion
    }
}
