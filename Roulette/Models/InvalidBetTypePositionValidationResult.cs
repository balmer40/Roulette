using System.ComponentModel.DataAnnotations;

namespace Roulette.Models
{
    public class InvalidBetTypePositionValidationResult : ValidationResult
    {
        public InvalidBetTypePositionValidationResult(int position, BetType betType) : base($"Invalid position {position} for betType {betType}")
        {

        }
    }
}
