using Roulette.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Roulette.Constants;
using Roulette.Exceptions;

namespace Roulette.Validators
{
    public class CornerBetTypeValidator : IBetTypeValidator
    {
        public ValidationResult ValidatePosition(int? position)
        {
            return position != null && Positions.CornerPositions.Contains(position.Value)
                ? ValidationResult.Success
                : new InvalidPositionValidationResult(position, BetType.Corner);
        }
    }
}
