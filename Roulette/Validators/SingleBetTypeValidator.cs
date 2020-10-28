using Roulette.Models;
using System.ComponentModel.DataAnnotations;

namespace Roulette.Validators
{
    public class SingleBetTypeValidator : IBetTypeValidator
    {
        public ValidationResult ValidateBetTypeAndPosition(BetType betType, int position)
        {
            return ValidationResult.Success;
        }
    }
}
