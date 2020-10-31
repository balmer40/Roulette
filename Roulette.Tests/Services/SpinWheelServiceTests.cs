using FluentAssertions;
using Roulette.Services;
using Xunit;

namespace Roulette.Tests.Services
{
    public class SpinWheelServiceTest
    {
        [Fact]
        public void GetWinningNumber_ReturnsWinningNumber()
        {
            var service = new SpinWheelService();

            var result = service.GetWinningNumber();

            result.Should().BeInRange(0, 36);
        }
    }
}
