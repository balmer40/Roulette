using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace Roulette.Tests.Helpers
{
    public class TestContext : IDisposable
    {

        private readonly TestServer _server;
        public readonly HttpClient Client;

        public TestContext()
        {
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>()
                .ConfigureServices(ConfigureTestServices));

            Client = _server.CreateClient();
        }

        private void ConfigureTestServices(IServiceCollection services)
        {

        }

        public void Dispose()
        {
            Client?.Dispose();
            _server?.Dispose();
        }
    }
}
