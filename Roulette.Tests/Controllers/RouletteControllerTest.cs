﻿using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Roulette.Controllers;
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
            var controller = new RouletteController(Substitute.For<ILogger<RouletteController>>(), mockService);

            await controller.NewGame();
            await mockService.Received().CreateNewGame();
        }

        [Fact]
        public async Task NewGame_ReturnsGameId()
        {
            var expectedGameId = Guid.NewGuid();
            var mockService = Substitute.For<IRouletteService>();
            mockService.CreateNewGame().Returns(expectedGameId);
            var controller = new RouletteController(Substitute.For<ILogger<RouletteController>>(), mockService);

            var result = await controller.NewGame() as OkNegotiatedContentResult<Guid>;

            result.Content.Should().Be(expectedGameId);
        }

        [Fact]
        public async Task NewGame_ReturnsInternalServerErrorWhenRouletteServiceThrows()
        {
            var mockService = Substitute.For<IRouletteService>();
            mockService.CreateNewGame().Throws(new Exception());
            var controller = new RouletteController(Substitute.For<ILogger<RouletteController>>(), mockService);

            var result = await controller.NewGame() as Microsoft.AspNetCore.Mvc.StatusCodeResult;

            result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }
    }
}
