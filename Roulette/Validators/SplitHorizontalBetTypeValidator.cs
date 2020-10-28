using Roulette.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Roulette.Validators
{
    public class SplitHorizontalBetTypeValidator : IBetTypeValidator
    {
        public ValidationResult ValidatePosition(int position = 0)
        {
            return Positions.SplitHorizontalPositions.Contains(position)
                ? ValidationResult.Success
                : new InvalidBetTypePositionValidationResult(position, BetType.SplitHorizontal);
        }
    }
}
