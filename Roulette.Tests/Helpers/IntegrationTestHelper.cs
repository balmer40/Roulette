using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Roulette.Models.Requests;
using Roulette.Models.Responses;

namespace Roulette.Tests.Helpers
{
    public class IntegrationTestHelper
    {
        private readonly TestContext _testContext;

        public IntegrationTestHelper(TestContext testContext)
        {
            _testContext = testContext;
        }

        public async Task<NewGameResponse> CreateNewGame()
        {
            var response = await _testContext.Client.PostAsync("api/roulette/new", null);

            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<NewGameResponse>(stringResponse);
        }

        public async Task CloseBetting(Guid gameId)
        {
            var response = await _testContext.Client.PutAsync($"api/roulette/{gameId}/close-betting", null);

            response.EnsureSuccessStatusCode();
        }

        public async Task<AddBetResponse> AddBet(Guid gameId, AddBetRequest request)
        {
            var response = await _testContext.Client.PostAsync(
                $"api/roulette/{gameId}/bet", 
                new StringContent(
                    JsonConvert.SerializeObject(request),
                    Encoding.UTF8,
                    "application/json"));

            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<AddBetResponse>(stringResponse);
        }
    }
}
