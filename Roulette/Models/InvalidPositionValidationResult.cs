using System.ComponentModel.DataAnnotations;

namespace Roulette.Models
{
    public class InvalidPositionValidationResult : ValidationResult
    {
        public InvalidPositionValidationResult(int? position, BetType betType) : base($"Invalid position '{position}' for betType: {betType}")
        {

        }
    }
}
