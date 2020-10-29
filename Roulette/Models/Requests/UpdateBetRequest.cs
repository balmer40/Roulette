using System;
using System.ComponentModel.DataAnnotations;

namespace Roulette.Models.Requests
{
    public class UpdateBetRequest
    {
        [Required]
        [Range(1, 10000)] //only allow maximum of 10k bet
        public double Amount { get; set; }
    }
}
