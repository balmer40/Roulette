using System;
using FluentAssertions;
using Roulette.Tests.Helpers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Roulette.Constants;
using Roulette.Models;
using Roulette.Models.Requests;
using Roulette.Models.Responses;
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
            var expectedCustomerId = Guid.NewGuid();
            var expectedAmount = 50.0;

            var newGameResponse = await _testHelper.CreateNewGame();

            await _testHelper.AddBet(
                newGameResponse.GameId,
                new AddBetRequest
                {
                    CustomerId = expectedCustomerId,
                    BetType = BetType.Black,
                    Position = null,
                    Amount = expectedAmount
                });

            await _testHelper.AddBet(
                newGameResponse.GameId,
                new AddBetRequest
                {
                    CustomerId = expectedCustomerId,
                    BetType = BetType.Red,
                    Position = null,
                    Amount = expectedAmount
                });

            await _testHelper.CloseBetting(newGameResponse.GameId);

            var playGameResponse = await _testContext.Client
                .PostAsync($"api/roulette/{newGameResponse.GameId}/play", null);

            playGameResponse.EnsureSuccessStatusCode();

            var stringResponse = await playGameResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<PlayGameResponse>(stringResponse);

            response.GameId.Should().Be(newGameResponse.GameId);
            response.WinningNumber.Should().BeInRange(0, 36);

            response.WinningBets.Count.Should().Be(1);
            response.WinningBets.Should().ContainKey(expectedCustomerId.ToString());
            response.WinningBets[expectedCustomerId.ToString()][0].AmountBet.Should().Be(expectedAmount);
            response.WinningBets[expectedCustomerId.ToString()][0].AmountWon.Should().Be(expectedAmount * BetMultipliers.BlackAndRedMultiplier);

            response.LosingBets.Count.Should().Be(1);
            response.LosingBets.Should().ContainKey(expectedCustomerId.ToString());
            response.LosingBets[expectedCustomerId.ToString()][0].AmountBet.Should().Be(expectedAmount);

            response.CustomerTotalWinnings.Count.Should().Be(1);
            response.CustomerTotalWinnings.Should().ContainKey(expectedCustomerId.ToString());
            response.CustomerTotalWinnings[expectedCustomerId.ToString()].Should()
                .Be(expectedAmount * BetMultipliers.BlackAndRedMultiplier);
        }

    }
}
