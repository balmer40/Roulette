using Roulette.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Roulette.Constants;

namespace Roulette.Validators
{
    public class ColumnBetTypeValidator : IBetTypeValidator
    {
        public ValidationResult ValidatePosition(int position = 0)
        {
            return Positions.ColumnPositions.Contains(position)
                ? ValidationResult.Success
                : new InvalidBetTypePositionValidationResult(position, BetType.Column);
        }
    }
}
