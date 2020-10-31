using FluentAssertions;
using Roulette.Exceptions;
using Roulette.Models;
using Roulette.Repositories;
using System;
using System.Threading.Tasks;
using NSubstitute;
using Roulette.Handlers;
using Roulette.Providers;
using Roulette.Validators;
using Xunit;

namespace Roulette.Tests.Providers
{
    public class BetTypeHandlerProviderTests
    {
        [Fact]
        public void GetBetTypeHandler_ReturnsBetTypeHandler()
        {
            var expectedBetTypeHandler = new SingleBetTypeHandler(Substitute.For<IBetTypeValidator>());
            var handlerProvider = new BetTypeHandlerProvider(new [] {expectedBetTypeHandler});

            var actualHandlerProvider = handlerProvider.GetBetTypeHandler(BetType.Single);

            actualHandlerProvider.Should().Be(expectedBetTypeHandler);
        }

        [Fact]
        public void GetBetTypeHandler_ThrowsWhenBetTypeNotConfigured()
        {
            var handlerProvider = new BetTypeHandlerProvider(new IBetTypeHandler[] {});

            Assert.Throws<BetTypeNotConfiguredException>(() => handlerProvider.GetBetTypeHandler(BetType.Single));
        }
    }
}
