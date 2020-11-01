using System.ComponentModel.DataAnnotations;

namespace Roulette.Models
{
    public class PositionNotAllowedValidationResult : ValidationResult
    {
        public PositionNotAllowedValidationResult(BetType betType) : base($"Position is not allowed for betType: {betType}")
        {

        }
    }
}
