using FluentAssertions;
using Roulette.Models;
using Roulette.Validators;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Roulette.Tests.Validators
{
    public class CornerBetTypeValidatorTest
    {
        public void ValidatePosition_ReturnsSuccessWhenValidPosition()
        {
            var validator = new CornerBetTypeValidator();

            var result = validator.ValidatePosition(Positions.CornerPositions[0]);

            result.Should().Be(ValidationResult.Success);
        }

        [Fact]
        public void ValidatePosition_ReturnsInvalidValidationResultWhenInvalidPosition()
        {
            var expectedPosition = 0;
            var validator = new CornerBetTypeValidator();

            var result = validator.ValidatePosition(expectedPosition);

            result.Should().BeOfType(typeof(InvalidBetTypePositionValidationResult));
            result.ErrorMessage.Should().Contain(BetType.Corner.ToString());
            result.ErrorMessage.Should().Contain(expectedPosition.ToString());
        }
    }
}
