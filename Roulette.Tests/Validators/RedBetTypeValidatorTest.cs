using FluentAssertions;
using Roulette.Models;
using Roulette.Validators;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Roulette.Tests.Validators
{
    public class RedBetTypeValidatorTest
    {
        [Fact]
        public void ValidatePosition_ReturnsSuccess()
        {
            var validator = new RedBetTypeValidator();

            var result = validator.ValidatePosition();

            result.Should().Be(ValidationResult.Success);
        }
    }
}
