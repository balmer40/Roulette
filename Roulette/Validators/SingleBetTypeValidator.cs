using Roulette.Models;
using System.ComponentModel.DataAnnotations;

namespace Roulette.Validators
{
    public class SingleBetTypeValidator : IBetTypeValidator
    {
        public ValidationResult ValidatePosition(int? position)
        {
            return position == null ? 
                new InvalidPositionValidationResult(position, BetType.Single) : 
                ValidationResult.Success;
        }
    }
}
