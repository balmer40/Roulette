using System.ComponentModel.DataAnnotations;

namespace Roulette.Validators
{
    public class RedBetTypeValidator : IBetTypeValidator
    {
        public ValidationResult ValidatePosition(int position = 0)
        {
            //position not needed for red bets
            return ValidationResult.Success;
        }
    }
}
