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
        public async Task NewGame_ReturnsGameId()
        {
            var expectedNewGameResponse = new NewGameResponse();
            var mockService = Substitute.For<IRouletteService>();
            mockService.CreateNewGame().Returns(expectedNewGameResponse);
            var controller = new RouletteController(mockService);

            var result = await controller.NewGame() as OkNegotiatedContentResult<NewGameResponse>;
            result.Content.Should().Be(expectedNewGameResponse);
        }

        #endregion

        #region CloseBets

        [Fact]
        public async Task CloseBets_ClosesBets()
        {
            var expectedGameId = Guid.NewGuid();
            var mockService = Substitute.For<IRouletteService>();
            var controller = new RouletteController(mockService);

            await controller.CloseBets(expectedGameId);
            await mockService.Received().CloseBets(expectedGameId);
        }

        #endregion

        #region AddBet

        [Fact]
        public async Task AddBet_AddsBet()
        {
            var expectedRequest = new AddBetRequest();
            var mockService = Substitute.For<IRouletteService>();
            var controller = new RouletteController(mockService);

            await controller.AddBet(expectedRequest);
            await mockService.Received().AddBet(expectedRequest);
        }

        #endregion

        #region DeleteBet

        [Fact]
        public async Task DeleteBet_DeletesBet()
        {
            var expectedBetId = Guid.NewGuid();
            var mockService = Substitute.For<IRouletteService>();
            var controller = new RouletteController(mockService);

            await controller.DeleteBet(expectedBetId);
            await mockService.Received().DeleteBet(expectedBetId);
        }

        #endregion
    }
}
