using System;

namespace Roulette.Models
{
    public class Game
    {
        public Guid Id { get; set; }
        public GameStatus GameStatus { get; set; } = GameStatus.GameOpen;
        public DateTime? OpenedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
    }

}
