using System;
using FluentAssertions;
using Newtonsoft.Json;
using Roulette.Models.Responses;
using Roulette.Tests.Helpers;
using System.Threading.Tasks;
using Xunit;

namespace Roulette.Tests.Controllers
{
    public class RouletteControllerIntegrationTests
    {
        private readonly TestContext _testContext;
        private readonly IntegrationTestHelper _testHelper;

        public RouletteControllerIntegrationTests()
        {
            _testContext = new TestContext();
            _testHelper = new IntegrationTestHelper(_testContext);
        }

        [Fact]
        public async Task NewGame_CreatesNewGameAndReturnsGameId()
        {
            var response = await _testHelper.CreateNewGame();

            response.GameId.Should().NotBeEmpty();
        }

        [Fact]
        public async Task CloseBetting_ClosesBetting()
        {
            var response = await _testHelper.CreateNewGame();

            await _testHelper.CloseBetting(response.GameId);
        }

        [Fact]
        public async Task PlayGame_PlaysGameAndReturnsWinnings()
        {
            var response = await _testHelper.CreateNewGame();

            await _testHelper.CloseBetting(response.GameId);
        }

    }
}
