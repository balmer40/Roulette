using System;

namespace Roulette.Models
{
    public class Game
    {
        public Guid Id { get; set; }
        public bool IsOpen { get; set; } = true;
        public DateTime? OpenedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
    }
}
