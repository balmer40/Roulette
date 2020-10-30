using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Roulette.Controllers;
using Roulette.Models.Responses;
using Roulette.Services;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Roulette.Models;
using Roulette.Models.Requests;
using Roulette.Tests.Helpers;
using Xunit;

namespace Roulette.Tests.Controllers
{
    public class BetControllerIntegrationTests
    {
        private readonly TestContext _testContext;
        private readonly IntegrationTestHelper _testHelper;

        public BetControllerIntegrationTests()
        {
            _testContext = new TestContext();
            _testHelper = new IntegrationTestHelper(_testContext);
        }

        [Fact]
        public async Task AddBet_AddsBet()
        {
            var newGameResponse = await _testHelper.CreateNewGame();

            var expectedCustomerId = Guid.NewGuid();
            var expectedBetType = BetType.Single;
            var expectedPosition = 1;
            var expectedAmount = 50.0;

            var addBetResponse = await _testHelper.AddBet(
                newGameResponse.GameId, 
                new AddBetRequest
                {
                    CustomerId = expectedCustomerId,
                    BetType = expectedBetType,
                    Position = expectedPosition,
                    Amount = expectedAmount
                });

            addBetResponse.Bet.Id.Should().NotBeEmpty();
            addBetResponse.Bet.CustomerId.Should().Be(expectedCustomerId);
            addBetResponse.Bet.GameId.Should().Be(newGameResponse.GameId);
            addBetResponse.Bet.BetType.Should().Be(expectedBetType);
            addBetResponse.Bet.Position.Should().Be(expectedPosition);
            addBetResponse.Bet.Amount.Should().Be(expectedAmount);
        }

        [Fact]
        public async Task UpdateBet_UpdatesBet()
        {
            var newGameResponse = await _testHelper.CreateNewGame();

            var expectedCustomerId = Guid.NewGuid();
            var expectedBetType = BetType.Single;
            var expectedPosition = 1;
            var expectedAmount = 50.0;

            var addBetResponse = await _testHelper.AddBet(
                newGameResponse.GameId,
                new AddBetRequest
                {
                    CustomerId = expectedCustomerId,
                    BetType = expectedBetType,
                    Position = expectedPosition,
                    Amount = 10.0
                });

            var response = await _testContext.Client.PutAsync(
                $"api/roulette/{newGameResponse.GameId}/bet/{addBetResponse.Bet.Id}",
                new StringContent(
                    JsonConvert.SerializeObject(new UpdateBetRequest {Amount = expectedAmount}),
                    Encoding.UTF8,
                    "application/json"));

            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var updatedBetResponse = JsonConvert.DeserializeObject<AddBetResponse>(stringResponse);

            updatedBetResponse.Bet.Id.Should().Be(addBetResponse.Bet.Id);
            updatedBetResponse.Bet.CustomerId.Should().Be(expectedCustomerId);
            updatedBetResponse.Bet.GameId.Should().Be(newGameResponse.GameId);
            updatedBetResponse.Bet.BetType.Should().Be(expectedBetType);
            updatedBetResponse.Bet.Position.Should().Be(expectedPosition);
            updatedBetResponse.Bet.Amount.Should().Be(expectedAmount);
        }
    }
}
