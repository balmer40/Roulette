using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Roulette.Models
{
    public abstract class BaseBet
    {
        public Guid Id { get; set; }
        public BetType BetType { get; set; }

        public int Position { get; set; }
    }
}
