using System;
using System.Text.Json.Serialization;

namespace Roulette.Models
{
    public abstract class BaseBet
    {
        public Guid Id { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public BetType BetType { get; set; }

        public int Position { get; set; }
    }
}
