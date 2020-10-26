using System;
using System.Collections.Generic;

namespace Roulette.Models.Responses
{
    public class PlayGameResponse
    {
        public Guid GameId { get; set; }
        public int WinningNumber { get; set; }
        public IDictionary<Guid, double> CustomerWinnings { get; set; }
    }
}
