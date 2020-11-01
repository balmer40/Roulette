using FluentAssertions;
using Roulette.Validators;
using System.ComponentModel.DataAnnotations;
using Roulette.Models;
using Xunit;

namespace Roulette.Tests.Validators
{
    public class SingleBetTypeValidatorTest
    {
        [Fact]
        public void ValidatePosition_ReturnsSuccess()
        {
            var validator = new SingleBetTypeValidator();

            var result = validator.ValidatePosition(1);

            result.Should().Be(ValidationResult.Success);
        }

        [Fact]
        public void ValidatePosition_ReturnsInvalidValidationResultWhenInvalidPosition()
        {
            int? expectedPosition = null;
            var validator = new SingleBetTypeValidator();

            var result = validator.ValidatePosition(expectedPosition);

            result.Should().BeOfType(typeof(InvalidPositionValidationResult));
            result.ErrorMessage.Should().Contain(BetType.Single.ToString());
        }
    }
}
