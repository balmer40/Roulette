using System;
using System.ComponentModel.DataAnnotations;

namespace Roulette.Models.Requests
{
    public class UpdateBetRequest
    {
        [Required]
        [Range(Ranges.MinimumBet, Ranges.MaximumBet - 1)] //can only update to max 10k, and the minimum bet already can be 1
        public double Amount { get; set; }
    }
}
