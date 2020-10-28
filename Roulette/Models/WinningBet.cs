using System;

namespace Roulette.Models
{
    public class WinningBet
    {
        public Guid Id { get; set; }
        public BetType BetType { get; set; }
        public int Position { get; set; }
        public double AmountBet { get; set; }
        public double AmountWon { get; set; }
    }
}
