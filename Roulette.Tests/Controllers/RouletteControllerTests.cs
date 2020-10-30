using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Roulette.Controllers;
using Roulette.Models.Responses;
using Roulette.Services;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Roulette.Tests.Controllers
{
    public class RouletteControllerTests
    {
        #region NewGame

        [Fact]
        public async Task NewGame_CreatesNewGame()
        {
            var mockService = Substitute.For<IGameService>();
            var controller = new RouletteController(mockService);

            await controller.NewGame();
            await mockService.Received().CreateNewGame();
        }

        [Fact]
        public async Task NewGame_ReturnsGameResponse()
        {
            var expectedNewGameResponse = new NewGameResponse();
            var mockService = Substitute.For<IGameService>();
            mockService.CreateNewGame().Returns(expectedNewGameResponse);
            var controller = new RouletteController(mockService);

            var result = await controller.NewGame() as OkObjectResult;
            result.Value.Should().Be(expectedNewGameResponse);
        }

        #endregion

        #region CloseBetting

        [Fact]
        public async Task CloseBetting_ClosesBetting()
        {
            var expectedGameId = Guid.NewGuid();
            var mockService = Substitute.For<IGameService>();
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
            var mockService = Substitute.For<IGameService>();
            var controller = new RouletteController(mockService);

            await controller.PlayGame(expectedGameId);
            await mockService.Received().PlayGame(expectedGameId);
        }

        [Fact]
        public async Task PlayGame_ReturnsPlayResponse()
        {
            var expectedPlayResponse = new PlayGameResponse();
            var mockService = Substitute.For<IGameService>();
            mockService.PlayGame(Arg.Any<Guid>()).Returns(expectedPlayResponse);
            var controller = new RouletteController(mockService);

            var result = await controller.PlayGame(Guid.NewGuid()) as OkObjectResult;
            result.Value.Should().Be(expectedPlayResponse);
        }

        #endregion
    }
}
