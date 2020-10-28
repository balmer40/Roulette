using FluentAssertions;
using Roulette.Validators;
using System.ComponentModel.DataAnnotations;
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
    }
}
