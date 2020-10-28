using Roulette.Models;
using System.ComponentModel.DataAnnotations;

namespace Roulette.Validators
{
    public class SplitVerticalBetTypeValidator : IBetTypeValidator
    {
        public ValidationResult ValidatePosition(int position = 0)
        {
            return position != 0
                ? ValidationResult.Success
                : new InvalidBetTypePositionValidationResult(position, BetType.SplitVertical);
        }
    }
}
