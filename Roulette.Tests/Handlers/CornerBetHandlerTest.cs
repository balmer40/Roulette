using FluentAssertions;
using NSubstitute;
using Roulette.Handlers;
using Roulette.Models;
using Roulette.Validators;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Roulette.Tests.Handlers
{
    public class CornerBetHandlerTest
    {
        [Fact]
        public void ValidatePosition_ValidatesPosition()
        {
            var expectedPosition = 1;
            var mockBetTypeValidator = Substitute.For<IBetTypeValidator>();
            var betHandler = new CornerBetHandler(mockBetTypeValidator);

            betHandler.ValidatePosition(expectedPosition);

            mockBetTypeValidator.Received().ValidatePosition(expectedPosition);
        }

        [Fact]
        public void ValidatePosition_ReturnsValidationResult()
        {
            var expectedValidationResult = ValidationResult.Success;
            var mockBetTypeValidator = Substitute.For<IBetTypeValidator>();
            mockBetTypeValidator.ValidatePosition(Arg.Any<int>()).Returns(expectedValidationResult);
            var betHandler = new CornerBetHandler(mockBetTypeValidator);

            var result = betHandler.ValidatePosition(1);

            result.Should().Be(expectedValidationResult);
        }

        [Theory]
        [InlineData(12, 12)]
        [InlineData(12, 11)]
        [InlineData(12, 9)]
        [InlineData(12, 8)]
        public void IsWinningBet_ReturnsTrueWhenWon(int winningNumber, int position)
        {
            var betHandler = new CornerBetHandler(Substitute.For<IBetTypeValidator>());

            var result = betHandler.IsWinningBet(winningNumber, position);

            result.Should().BeTrue();
        }

        [Fact]
        public void IsWinningBet_ReturnsFalseWhenLost()
        {
            var betHandler = new CornerBetHandler(Substitute.For<IBetTypeValidator>());

            var result = betHandler.IsWinningBet(12, 13);

            result.Should().BeFalse();
        }

        [Fact]
        public void CalculatesWinnings_ReturnsExpectedWinnings()
        {
            var amount = 1;
            var betHandler = new CornerBetHandler(Substitute.For<IBetTypeValidator>());

            var result = betHandler.CalculateWinnings(new Bet { Amount = amount });

            result.Should().Be(amount * BetMultipliers.CornerMultiplier);
        }
    }
}
