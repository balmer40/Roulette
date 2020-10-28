using Roulette.Models;
using System.ComponentModel.DataAnnotations;

namespace Roulette.Validators
{
    public interface IBetTypeValidator
    {
        ValidationResult ValidatePosition(int position);
    }
}
