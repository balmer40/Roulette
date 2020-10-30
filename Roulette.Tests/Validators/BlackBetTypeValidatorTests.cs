using FluentAssertions;
using Roulette.Models;
using Roulette.Validators;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Roulette.Tests.Validators
{
    public class BlackBetTypeValidatorTests
    {
        [Fact]
        public void ValidatePosition_ReturnsSuccess()
        {
            var validator = new BlackBetTypeValidator();

            var result = validator.ValidatePosition();

            result.Should().Be(ValidationResult.Success);
        }
    }
}
