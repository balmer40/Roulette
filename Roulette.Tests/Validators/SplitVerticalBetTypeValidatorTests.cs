using FluentAssertions;
using Roulette.Models;
using Roulette.Validators;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Roulette.Tests.Validators
{
    public class SplitVerticalBetTypeValidatorTests
    {
        [Fact]
        public void ValidatePosition_ReturnsSuccessWhenValidPosition()
        {
            var validator = new SplitVerticalBetTypeValidator();

            var result = validator.ValidatePosition(1);

            result.Should().Be(ValidationResult.Success);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(0)]
        public void ValidatePosition_ReturnsInvalidValidationResultWhenInvalidPosition(int? expectedPosition)
        {
            var validator = new SplitVerticalBetTypeValidator();

            var result = validator.ValidatePosition(expectedPosition);

            result.Should().BeOfType(typeof(InvalidPositionValidationResult));
            result.ErrorMessage.Should().Contain(BetType.SplitVertical.ToString());
        }
    }
}
