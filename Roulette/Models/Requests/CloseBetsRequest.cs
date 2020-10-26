using System;
using System.ComponentModel.DataAnnotations;

namespace Roulette.Models.Requests
{
    public class CloseBetsRequest
    {
        [Required]
        public Guid GameId { get; }
    }
}
