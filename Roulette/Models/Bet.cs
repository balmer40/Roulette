using System;

namespace Roulette.Models
{
    public class Bet
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public BetType BetType { get; }
        public int Position { get; }
        public double Amount { get; }
    }
}
