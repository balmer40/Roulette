using System;

namespace Roulette.Exceptions
{
    public class GameBettingOpenException : GameStatusException
    {
        public GameBettingOpenException(Guid id) : base($"Betting for game with id '{id}' is open and needs to be closed before playing")
        {

        }
    }
}
