﻿using FluentAssertions;
using Roulette.Models;
using Roulette.Validators;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Roulette.Tests.Validators
{
    public class RedBetTypeValidatorTests
    {
        [Fact]
        public void ValidatePosition_ReturnsSuccess()
        {
            var validator = new RedBetTypeValidator();

            var result = validator.ValidatePosition(null);

            result.Should().Be(ValidationResult.Success);
        }

        [Fact]
        public void ValidatePosition_ReturnsPositionNotAllowedResult()
        {
            var validator = new RedBetTypeValidator();

            var result = validator.ValidatePosition(1);

            result.Should().BeOfType(typeof(PositionNotAllowedValidationResult));
            result.ErrorMessage.Should().Contain(BetType.Red.ToString());
        }
    }
}
