using System.ComponentModel.DataAnnotations;

namespace Roulette.Validators
{
    public class BlackBetTypeValidator : IBetTypeValidator
    {
        public ValidationResult ValidatePosition(int position = 0)
        {
            //position not needed for black bets
            return ValidationResult.Success;
        }
    }
}
