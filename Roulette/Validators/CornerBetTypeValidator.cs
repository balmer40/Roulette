using Roulette.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Roulette.Validators
{
    public class CornerBetTypeValidator : IBetTypeValidator
    {
        public ValidationResult ValidatePosition(int position = 0)
        {
            return Positions.CornerPositions.Contains(position)
                ? ValidationResult.Success
                : new InvalidBetTypePositionValidationResult(position, BetType.Corner);
        }
    }
}
