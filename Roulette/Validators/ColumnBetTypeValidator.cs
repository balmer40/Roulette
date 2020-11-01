using Roulette.Constants;
using Roulette.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Roulette.Validators
{
    public class ColumnBetTypeValidator : IBetTypeValidator
    {
        public ValidationResult ValidatePosition(int? position)
        {

            return position != null && Positions.ColumnPositions.Contains(position.Value)
                ? ValidationResult.Success
                : new InvalidPositionValidationResult(position, BetType.Column);
        }
    }
}
