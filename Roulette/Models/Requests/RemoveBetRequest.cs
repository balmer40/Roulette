using System;
using System.ComponentModel.DataAnnotations;

namespace Roulette.Models.Requests
{
    public class RemoveBetRequest
    {
        [Required]
        public Guid GameId { get; }

        [Required]
        public Guid CustomerId { get; }

        [Required]
        public BetType BetType { get; }

        [Range(0,36)]
        public int Position { get; }

        [Range(0, 10000)] //only allow maximum of 10k bet
        public double Amount { get; }
    }
}
