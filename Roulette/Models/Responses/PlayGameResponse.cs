using System;
using System.Collections.Generic;

namespace Roulette.Models.Responses
{
    public class PlayGameResponse
    {
        public Guid GameId { get; set; }
        public int WinningNumber { get; set; }
        public Dictionary<string, WinningBet[]> WinningBets { get; set; }
        public Dictionary<string, double> CustomerTotalWinnings { get; set; }
    }
}
