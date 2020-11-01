using Roulette.Models;
using System.ComponentModel.DataAnnotations;

namespace Roulette.Validators
{
    public class BlackBetTypeValidator : IBetTypeValidator
    {
        public ValidationResult ValidatePosition(int? position)
        {
            return position != null ? new PositionNotAllowedValidationResult(BetType.Black) : ValidationResult.Success;
        }
    }
}
