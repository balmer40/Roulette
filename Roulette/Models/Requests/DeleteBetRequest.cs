using System;
using System.ComponentModel.DataAnnotations;

namespace Roulette.Models.Requests
{
    public class DeleteBetRequest
    {
        [Required]
        public Guid GameId { get; set; }

        [Required]
        public Guid BetId { get; set; }
    }
}
