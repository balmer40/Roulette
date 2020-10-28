using System;

namespace Roulette.Models
{
    public class Bet
    {
        public Guid Id { get; set; }
        public Guid GameId { get; set; }
        public Guid CustomerId { get; set; }
        public BetType BetType { get; set; }
        public int Position { get; set; }
        public double Amount { get; set; }
    }
}
