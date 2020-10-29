using System;

namespace Roulette.Models
{
    public class Bet : BaseBet
    {
        public Guid GameId { get; set; }

        public Guid CustomerId { get; set; }

        public double Amount { get; set; }
    }
}
