﻿using System;
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
    public class SplitVerticalBetTypeHandlerTest
    {
        [Fact]
        public void ValidatePosition_ValidatesPosition()
        {
            var expectedPosition = 1;
            var mockBetTypeValidator = Substitute.For<IBetTypeValidator>();
            var betTypeHandler = new SplitVerticalBetTypeHandler(mockBetTypeValidator);

            betTypeHandler.ValidatePosition(expectedPosition);

            mockBetTypeValidator.Received().ValidatePosition(expectedPosition);
        }

        [Fact]
        public void ValidatePosition_ReturnsValidationResult()
        {
            var expectedValidationResult = ValidationResult.Success;
            var mockBetTypeValidator = Substitute.For<IBetTypeValidator>();
            mockBetTypeValidator.ValidatePosition(Arg.Any<int>()).Returns(expectedValidationResult);
            var betTypeHandler = new SplitVerticalBetTypeHandler(mockBetTypeValidator);

            var result = betTypeHandler.ValidatePosition(1);

            result.Should().Be(expectedValidationResult);
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 1)]
        [InlineData(0, 2)]
        [InlineData(2, 2)]
        [InlineData(3, 6)]
        [InlineData(6, 6)]
        public void IsWinningBet_ReturnsTrueWhenWon(int winningNumber, int position)
        {
            var betTypeHandler = new SplitVerticalBetTypeHandler(Substitute.For<IBetTypeValidator>());

            var result = betTypeHandler.IsWinningBet(winningNumber, position);

            result.Should().BeTrue();
        }

        [Fact]
        public void IsWinningBet_ReturnsFalseWhenLost()
        {
            var betTypeHandler = new SplitVerticalBetTypeHandler(Substitute.For<IBetTypeValidator>());

            var result = betTypeHandler.IsWinningBet(3, 1);

            result.Should().BeFalse();
        }

        [Fact]
        public void CalculatesWinnings_ReturnsExpectedWinnings()
        {
            var expectedId = Guid.NewGuid();
            var expectedBetType = BetType.Black;
            var expectedPosition = 1;
            var expectedAmount = 50.0;
            var betTypeHandler = new SplitVerticalBetTypeHandler(Substitute.For<IBetTypeValidator>());

            var result = betTypeHandler.CalculateWinnings(
                new Bet
                {
                    Id = expectedId,
                    BetType = expectedBetType,
                    Position = expectedPosition,
                    Amount = expectedAmount
                });

            result.Should().BeOfType<WinningBet>();
            result.Id.Should().Be(expectedId);
            result.BetType.Should().Be(expectedBetType);
            result.Position.Should().Be(expectedPosition);
            result.AmountBet.Should().Be(expectedAmount);
            result.AmountWon.Should().Be(expectedAmount * BetMultipliers.SplitMultiplier);
        }
    }
}
