using Roulette.Constants;
using Roulette.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Roulette.Validators
{
    public class SplitHorizontalBetTypeValidator : IBetTypeValidator
    {
        public ValidationResult ValidatePosition(int? position)
        {
            return position != null && Positions.SplitHorizontalPositions.Contains(position.Value)
                ? ValidationResult.Success
                : new InvalidPositionValidationResult(position, BetType.SplitHorizontal);
        }
    }
}
