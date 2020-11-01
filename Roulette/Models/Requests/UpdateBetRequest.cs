using Roulette.Constants;
using System.ComponentModel.DataAnnotations;

namespace Roulette.Models.Requests
{
    public class UpdateBetRequest
    {
        [Required]
        [Range(Ranges.MinimumAmount, Ranges.MaximumAmount)]
        [RegularExpression(RegularExpressions.Amount, ErrorMessage = "Amount can only have two decimal places")]
        public double Amount { get; set; }
    }
}
