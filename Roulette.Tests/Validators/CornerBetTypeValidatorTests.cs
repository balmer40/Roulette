using FluentAssertions;
using Roulette.Models;
using Roulette.Validators;
using System.ComponentModel.DataAnnotations;
using Roulette.Constants;
using Xunit;

namespace Roulette.Tests.Validators
{
    public class CornerBetTypeValidatorTests
    {
        [Fact]
        public void ValidatePosition_ReturnsSuccessWhenValidPosition()
        {
            var validator = new CornerBetTypeValidator();

            var result = validator.ValidatePosition(Positions.CornerPositions[0]);

            result.Should().Be(ValidationResult.Success);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(0)]
        public void ValidatePosition_ReturnsInvalidValidationResultWhenInvalidPosition(int? expectedPosition)
        {
            var validator = new CornerBetTypeValidator();

            var result = validator.ValidatePosition(expectedPosition);

            result.Should().BeOfType(typeof(InvalidPositionValidationResult));
            result.ErrorMessage.Should().Contain(BetType.Corner.ToString());
        }
    }
}
