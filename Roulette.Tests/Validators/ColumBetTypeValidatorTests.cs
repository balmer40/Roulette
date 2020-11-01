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


        [Theory]
        [InlineData(null)]
        [InlineData(0)]
        public void ValidatePosition_ReturnsInvalidValidationResultWhenInvalidPosition(int? expectedPosition)
        {
            var validator = new ColumnBetTypeValidator();

            var result = validator.ValidatePosition(expectedPosition);

            result.Should().BeOfType(typeof(InvalidPositionValidationResult));
            result.ErrorMessage.Should().Contain(BetType.Column.ToString());
        }
    }
}
