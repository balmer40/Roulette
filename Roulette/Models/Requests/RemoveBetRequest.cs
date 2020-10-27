using System;
using System.ComponentModel.DataAnnotations;

namespace Roulette.Models.Requests
{
    public class RemoveBetRequest
    {
        [Required]
        public Guid BetId { get; }
    }
}
