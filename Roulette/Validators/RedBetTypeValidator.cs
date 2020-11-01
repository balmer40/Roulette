using Roulette.Models;
using System.ComponentModel.DataAnnotations;

namespace Roulette.Validators
{
    public class RedBetTypeValidator : IBetTypeValidator
    {
        public ValidationResult ValidatePosition(int? position)
        {
            return position != null ? new PositionNotAllowedValidationResult(BetType.Red) : ValidationResult.Success;
        }
    }
}
