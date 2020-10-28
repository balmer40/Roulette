using System;

namespace Roulette.Exceptions
{
    public class GameBettingClosedException : GameStatusException
    {
        public GameBettingClosedException(Guid id) : base($"Betting is closed for game with id: {id}")
        {

        }
    }
}
