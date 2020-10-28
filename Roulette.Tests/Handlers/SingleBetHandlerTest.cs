﻿using FluentAssertions;
using NSubstitute;
using Roulette.Handlers;
using Roulette.Models;
using Roulette.Validators;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Roulette.Tests.Handlers
{
    public class SingleBetHandlerTest
    {
        [Fact]
        public void ValidatePosition_ValidatesPosition()
        {
            var mockBetTypeValidator = Substitute.For<IBetTypeValidator>();
            var betHandler = new SingleBetHandler(mockBetTypeValidator);

            betHandler.ValidatePosition();

            mockBetTypeValidator.Received().ValidatePosition();
        }

        [Fact]
        public void ValidatePosition_ReturnsValidationResult()
        {
            var expectedValidationResult = ValidationResult.Success;
            var mockBetTypeValidator = Substitute.For<IBetTypeValidator>();
            mockBetTypeValidator.ValidatePosition(Arg.Any<int>()).Returns(expectedValidationResult);
            var betHandler = new SingleBetHandler(mockBetTypeValidator);

            var result = betHandler.ValidatePosition(1);

            result.Should().Be(expectedValidationResult);
        }

        [Fact]
        public void IsWinningBet_ReturnsTrueWhenWon()
        {
            var betHandler = new SingleBetHandler(Substitute.For<IBetTypeValidator>());

            var result = betHandler.IsWinningBet(1, 1);

            result.Should().BeTrue();
        }

        [Fact]
        public void IsWinningBet_ReturnsFalseWhenLost()
        {
            var betHandler = new SingleBetHandler(Substitute.For<IBetTypeValidator>());

            var result = betHandler.IsWinningBet(1, 2);

            result.Should().BeFalse();
        }

        [Fact]
        public void CalculatesWinnings_ReturnsExpectedWinnings()
        {
            var amount = 1;
            var betHandler = new SingleBetHandler(Substitute.For<IBetTypeValidator>());

            var result = betHandler.CalculateWinnings(new Bet { Amount = amount });

            result.Should().Be(amount * BetMultipliers.SingleMultiplier);
        }
    }
}
