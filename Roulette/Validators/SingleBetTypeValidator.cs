using System.ComponentModel.DataAnnotations;

namespace Roulette.Validators
{
    public class SingleBetTypeValidator : IBetTypeValidator
    {
        public ValidationResult ValidatePosition(int position = 0)
        {
            // single bet can be any position
            return ValidationResult.Success;
        }
    }
}
