using System;
using FluentAssertions;
using NSubstitute;
using Roulette.Handlers;
using Roulette.Models;
using Roulette.Validators;
using System.ComponentModel.DataAnnotations;
using Roulette.Constants;
using Xunit;

namespace Roulette.Tests.Handlers
{
    public class BlackBetTypeHandlerTests
    {
        [Fact]
        public void ValidatePosition_ValidatesPosition()
        {
            var mockBetTypeValidator = Substitute.For<IBetTypeValidator>();
            var betTypeHandler = new BlackBetTypeHandler(mockBetTypeValidator);

            betTypeHandler.ValidatePosition(null);

            mockBetTypeValidator.Received().ValidatePosition(null);
        }

        [Fact]
        public void ValidatePosition_ReturnsSuccessResult()
        {
            var expectedValidationResult = ValidationResult.Success;
            var mockBetTypeValidator = Substitute.For<IBetTypeValidator>();
            mockBetTypeValidator.ValidatePosition(Arg.Any<int>()).Returns(expectedValidationResult);
            var betTypeHandler = new BlackBetTypeHandler(mockBetTypeValidator);

            var result = betTypeHandler.ValidatePosition(null);

            result.Should().Be(expectedValidationResult);
        }

        [Fact]
        public void ValidatePosition_ReturnsPositionNotAllowedResult()
        {
            var expectedValidationResult = new PositionNotAllowedValidationResult(BetType.Black);
            var mockBetTypeValidator = Substitute.For<IBetTypeValidator>();
            mockBetTypeValidator.ValidatePosition(Arg.Any<int>()).Returns(expectedValidationResult);
            var betTypeHandler = new BlackBetTypeHandler(mockBetTypeValidator);

            var result = betTypeHandler.ValidatePosition(1);

            result.Should().Be(expectedValidationResult);
        }

        [Fact]
        public void IsWinningBet_ReturnsTrueWhenWon()
        {
            var betTypeHandler = new BlackBetTypeHandler(Substitute.For<IBetTypeValidator>());

            var result = betTypeHandler.IsWinningBet(2);

            result.Should().BeTrue();
        }

        [Fact]
        public void IsWinningBet_ReturnsFalseWhenLost()
        {
            var betTypeHandler = new BlackBetTypeHandler(Substitute.For<IBetTypeValidator>());

            var result = betTypeHandler.IsWinningBet(3);

            result.Should().BeFalse();
        }

        [Fact]
        public void CalculatesWinnings_ReturnsExpectedWinnings()
        {
            
            var expectedId = Guid.NewGuid();
            var expectedBetType = BetType.Black;
            var expectedPosition = 1;
            var expectedAmount = 50.0;
            var betTypeHandler = new BlackBetTypeHandler(Substitute.For<IBetTypeValidator>());

            var result = betTypeHandler.CalculateWinnings(
                new Bet
                {
                    Id = expectedId, BetType = expectedBetType, Position = expectedPosition, Amount = expectedAmount
                });

            result.Should().BeOfType<WinningBet>();
            result.Id.Should().Be(expectedId);
            result.BetType.Should().Be(expectedBetType);
            result.Position.Should().Be(expectedPosition);
            result.AmountBet.Should().Be(expectedAmount);
            result.AmountWon.Should().Be(expectedAmount * BetMultipliers.BlackAndRedMultiplier);
        }
    }
}
