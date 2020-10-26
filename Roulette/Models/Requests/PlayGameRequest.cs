using System;
using System.ComponentModel.DataAnnotations;

namespace Roulette.Models.Requests
{
    public class PlayGameRequest
    {
        [Required]
        public Guid GameId { get; set; }
    }
}
