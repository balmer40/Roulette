using Roulette.Models;
using System.ComponentModel.DataAnnotations;

namespace Roulette.Validators
{
    public class SplitVerticalBetTypeValidator : IBetTypeValidator
    {
        public ValidationResult ValidatePosition(int? position)
        {
            return position != null && position != 0
                ? ValidationResult.Success
                : new InvalidPositionValidationResult(position, BetType.SplitVertical);
        }
    }
}
