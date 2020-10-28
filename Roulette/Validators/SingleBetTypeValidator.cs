using Roulette.Models;
using System.ComponentModel.DataAnnotations;

namespace Roulette.Validators
{
    public class SingleBetTypeValidator : IBetTypeValidator
    {
        public ValidationResult ValidatePosition(int position)
        {
            // single bet can be any position
            return ValidationResult.Success;
        }
    }
}
