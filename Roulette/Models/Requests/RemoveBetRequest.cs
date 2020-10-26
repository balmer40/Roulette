using System;

namespace Roulette.Models.Requests
{
    public class RemoveBetRequest
    {
        public Guid GameId { get; }
        public Guid CustomerId { get; }
        public BetType BetType { get; }
        public int Position { get; }
        public double Amount { get; }
    }
}
