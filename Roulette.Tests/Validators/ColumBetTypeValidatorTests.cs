using FluentAssertions;
using Roulette.Models;
using Roulette.Validators;
using System.ComponentModel.DataAnnotations;
using Roulette.Constants;
using Xunit;

namespace Roulette.Tests.Validators
{
    public class ColumnBetTypeValidatorTest
    {
        [Fact]
        public void ValidatePosition_ReturnsSuccessWhenValidPosition()
        {
            var validator = new ColumnBetTypeValidator();

            var result = validator.ValidatePosition(Positions.ColumnPositions[0]);

            result.Should().Be(ValidationResult.Success);
        }

        [Fact]
        public void ValidatePosition_ReturnsInvalidValidationResultWhenInvalidPosition()
        {
            var expectedPosition = 0;
            var validator = new ColumnBetTypeValidator();

            var result = validator.ValidatePosition(expectedPosition);

            result.Should().BeOfType(typeof(InvalidBetTypePositionValidationResult));
            result.ErrorMessage.Should().Contain(BetType.Column.ToString());
            result.ErrorMessage.Should().Contain(expectedPosition.ToString());
        }
    }
}
