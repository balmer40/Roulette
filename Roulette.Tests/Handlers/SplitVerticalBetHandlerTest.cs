using FluentAssertions;
using NSubstitute;
using Roulette.Handlers;
using Roulette.Models;
using Roulette.Validators;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Roulette.Tests.Handlers
{
    public class SplitVerticalBetHandlerTest
    {
        [Fact]
        public void ValidatePosition_ValidatesPosition()
        {
            var expectedPosition = 1;
            var mockBetTypeValidator = Substitute.For<IBetTypeValidator>();
            var betHandler = new SplitVerticalBetHandler(mockBetTypeValidator);

            betHandler.ValidatePosition(expectedPosition);

            mockBetTypeValidator.Received().ValidatePosition(expectedPosition);
        }

        [Fact]
        public void ValidatePosition_ReturnsValidationResult()
        {
            var expectedValidationResult = ValidationResult.Success;
            var mockBetTypeValidator = Substitute.For<IBetTypeValidator>();
            mockBetTypeValidator.ValidatePosition(Arg.Any<int>()).Returns(expectedValidationResult);
            var betHandler = new SplitVerticalBetHandler(mockBetTypeValidator);

            var result = betHandler.ValidatePosition(1);

            result.Should().Be(expectedValidationResult);
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(2, 0)]
        [InlineData(6, 3)]
        public void IsWinningBet_ReturnsTrueWhenWon(int winningNumber, int position)
        {
            var betHandler = new SplitVerticalBetHandler(Substitute.For<IBetTypeValidator>());

            var result = betHandler.IsWinningBet(winningNumber, position);

            result.Should().BeTrue();
        }

        [Fact]
        public void IsWinningBet_ReturnsFalseWhenLost()
        {
            var betHandler = new SplitVerticalBetHandler(Substitute.For<IBetTypeValidator>());

            var result = betHandler.IsWinningBet(3, 1);

            result.Should().BeFalse();
        }

        [Fact]
        public void CalculatesWinnings_ReturnsExpectedWinnings()
        {
            var amount = 1;
            var betHandler = new SplitVerticalBetHandler(Substitute.For<IBetTypeValidator>());

            var result = betHandler.CalculateWinnings(new Bet { Amount = amount });

            result.Should().Be(amount * BetMultipliers.SplitMultiplier);
        }
    }
}
