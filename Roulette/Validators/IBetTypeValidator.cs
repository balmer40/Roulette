using System.ComponentModel.DataAnnotations;

namespace Roulette.Validators
{
    public interface IBetTypeValidator
    {
        ValidationResult ValidatePosition(int position = 0);
    }
}
