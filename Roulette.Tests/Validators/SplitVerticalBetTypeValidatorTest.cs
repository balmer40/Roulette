using FluentAssertions;
using Roulette.Models;
using Roulette.Validators;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Roulette.Tests.Validators
{
    public class SplitVerticalBetTypeValidatorTest
    {
        [Fact]
        public void ValidatePosition_ReturnsSuccessWhenValidPosition()
        {
            var validator = new SplitVerticalBetTypeValidator();

            var result = validator.ValidatePosition(1);

            result.Should().Be(ValidationResult.Success);
        }

        [Fact]
        public void ValidatePosition_ReturnsInvalidValidationResultWhenInvalidPosition()
        {
            var expectedPosition = 0;
            var validator = new SplitVerticalBetTypeValidator();

            var result = validator.ValidatePosition(expectedPosition);

            result.Should().BeOfType(typeof(InvalidBetTypePositionValidationResult));
            result.ErrorMessage.Should().Contain(BetType.SplitVertical.ToString());
            result.ErrorMessage.Should().Contain(expectedPosition.ToString());
        }
    }
}
