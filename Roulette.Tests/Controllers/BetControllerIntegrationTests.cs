using FluentAssertions;
using Newtonsoft.Json;
using Roulette.Models;
using Roulette.Models.Requests;
using Roulette.Models.Responses;
using Roulette.Tests.Helpers;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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

        [Theory]
        [InlineData(BetType.Single, 1)]
        [InlineData(BetType.Red, null)]
        [InlineData(BetType.Black, null)]
        [InlineData(BetType.Corner, 12)]
        [InlineData(BetType.Column, 1)]
        [InlineData(BetType.SplitHorizontal, 2)]
        [InlineData(BetType.SplitVertical, 2)]
        public async Task AddBet_AddsBet(BetType expectedBetType, int? expectedPosition)
        {
            var newGameResponse = await _testHelper.CreateNewGame();

            var expectedCustomerId = Guid.NewGuid();
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

        [Theory]
        [InlineData(BetType.Single, null)]
        [InlineData(BetType.Red, 1)]
        [InlineData(BetType.Black, 1)]
        [InlineData(BetType.Corner, null)]
        [InlineData(BetType.Column, null)]
        [InlineData(BetType.SplitHorizontal, null)]
        [InlineData(BetType.SplitVertical, null)]
        public async Task AddBet_ReturnsBadRequestWhenInvalidPosition(BetType betType, int? position)
        {
            var newGameResponse = await _testHelper.CreateNewGame();

            var request = new AddBetRequest
            {
                CustomerId = Guid.NewGuid(),
                BetType = betType,
                Position = position,
                Amount = 100.0
            };

            var response = await _testContext.Client.PostAsync(
                $"api/roulette/{newGameResponse.GameId}/bet",
                new StringContent(
                    JsonConvert.SerializeObject(request),
                    Encoding.UTF8,
                    "application/json"));

            response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Theory]
        [InlineData(null, 1, 4)] // empty customerId
        [InlineData("b9cd97d2-3617-4a22-b6de-98379f13a12f", -1, 4)] // position out of range
        [InlineData("b9cd97d2-3617-4a22-b6de-98379f13a12f", 37, 4)] // position out of range
        [InlineData("b9cd97d2-3617-4a22-b6de-98379f13a12f", 1, 0)] // amount out of range
        [InlineData("b9cd97d2-3617-4a22-b6de-98379f13a12f", 1, 10001)] // amount out of range
        [InlineData("b9cd97d2-3617-4a22-b6de-98379f13a12f", 1, 15.5768)] // more than 2 decimal places for amount
        public async Task AddBet_ReturnsBadRequestWhenInvalidFieldsOnRequest(string customerId, int position, double amount)
        {
            var newGameResponse = await _testHelper.CreateNewGame();

            var request = new AddBetRequest
            {
                CustomerId = customerId == null ? Guid.Empty : Guid.Parse(customerId),
                BetType = BetType.Single,
                Position = position,
                Amount = amount
            };

            var response = await _testContext.Client.PostAsync(
                $"api/roulette/{newGameResponse.GameId}/bet",
                new StringContent(
                    JsonConvert.SerializeObject(request),
                    Encoding.UTF8,
                    "application/json"));

            response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
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
                    JsonConvert.SerializeObject(new UpdateBetRequest { Amount = expectedAmount }),
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

        [Theory]
        [InlineData(0)] // amount out of range
        [InlineData(10001)] // amount out of range
        [InlineData(15.1234)] // more than 2 decimal places for amount
        public async Task UpdateBet_ReturnsBadRequestWhenFieldsOnRequestAreInvalid(double amount)
        {
            var newGameResponse = await _testHelper.CreateNewGame();

            var addBetResponse = await _testHelper.AddBet(
                newGameResponse.GameId,
                new AddBetRequest
                {
                    CustomerId = Guid.NewGuid(),
                    BetType = BetType.Single,
                    Position = 1,
                    Amount = 100
                });

            var response = await _testContext.Client.PutAsync(
                $"api/roulette/{newGameResponse.GameId}/bet/{addBetResponse.Bet.Id}",
                new StringContent(
                    JsonConvert.SerializeObject(new UpdateBetRequest { Amount = amount }),
                    Encoding.UTF8,
                    "application/json"));

            response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task DeleteBet_DeletesBet()
        {
            var newGameResponse = await _testHelper.CreateNewGame();

            var addBetResponse = await _testHelper.AddBet(
                newGameResponse.GameId,
                new AddBetRequest
                {
                    CustomerId = Guid.NewGuid(),
                    BetType = BetType.Single,
                    Position = 1,
                    Amount = 10.0
                });

            var response = await _testContext.Client.DeleteAsync(
                $"api/roulette/{newGameResponse.GameId}/bet/{addBetResponse.Bet.Id}");

            response.EnsureSuccessStatusCode();
        }
    }
}
