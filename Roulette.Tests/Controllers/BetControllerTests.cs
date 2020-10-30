using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Roulette.Controllers;
using Roulette.Models.Requests;
using Roulette.Models.Responses;
using Roulette.Services;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Roulette.Tests.Controllers
{
    public class BetControllerTests
    {
        #region AddBet

        [Fact]
        public async Task AddBet_AddsBet()
        {
            var expectedGameId = Guid.NewGuid();
            var expectedRequest = new AddBetRequest();
            var mockService = Substitute.For<IBetService>();
            var controller = new BetController(mockService);

            await controller.AddBet(expectedGameId, expectedRequest);

            await mockService.Received().AddBet(expectedGameId, expectedRequest);
        }

        [Fact]
        public async Task AddBet_ReturnsExpectedResponse()
        {
            var expectedBetResponse = new AddBetResponse();
            var mockService = Substitute.For<IBetService>();
            mockService.AddBet(Arg.Any<Guid>(), Arg.Any<AddBetRequest>()).Returns(expectedBetResponse);
            var controller = new BetController(mockService);

            var result = await controller.AddBet(Guid.NewGuid(), new AddBetRequest()) as OkObjectResult;

            result.Value.Should().Be(expectedBetResponse);
        }

        #endregion

        #region UpdateBet

        [Fact]
        public async Task UpdateBet_UpdatesBet()
        {
            var expectedGameId = Guid.NewGuid();
            var expectedBetId = Guid.NewGuid();
            var expectedRequest = new UpdateBetRequest();
            var mockService = Substitute.For<IBetService>();
            var controller = new BetController(mockService);

            await controller.UpdateBet(expectedGameId, expectedBetId, expectedRequest);

            await mockService.Received().UpdateBet(expectedGameId, expectedBetId, expectedRequest);
        }

        [Fact]
        public async Task UpdateBet_ReturnsExpectedResponse()
        {
            var expectedBetResponse = new UpdateBetResponse();
            var mockService = Substitute.For<IBetService>();
            mockService.UpdateBet(Arg.Any<Guid>(), Arg.Any<Guid>(), Arg.Any<UpdateBetRequest>()).Returns(expectedBetResponse);
            var controller = new BetController(mockService);

            var result = await controller.UpdateBet(Guid.NewGuid(), Guid.NewGuid(), new UpdateBetRequest()) as OkObjectResult;

            result.Value.Should().Be(expectedBetResponse);
        }

        #endregion

        #region DeleteBet

        [Fact]
        public async Task DeleteBet_DeletesBet()
        {
            var expectedGameId = Guid.NewGuid();
            var expectedBetId = Guid.NewGuid();
            var mockService = Substitute.For<IBetService>();
            var controller = new BetController(mockService);

            await controller.DeleteBet(expectedGameId, expectedBetId);

            await mockService.Received().DeleteBet(expectedGameId, expectedBetId);
        }

        #endregion
    }
}
