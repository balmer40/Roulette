using Roulette.Models;
using System.ComponentModel.DataAnnotations;

namespace Roulette.Validators
{
    public interface IBetTypeValidator
    {
        ValidationResult ValidateBetTypeAndPosition(BetType betType, int position);
    }
}
