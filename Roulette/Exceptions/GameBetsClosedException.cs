using System;

namespace Roulette.Exceptions
{
    public class GameBetsClosedException : Exception
    {
        public GameBetsClosedException(Guid id) : base($"Betting is closed for game with id: {id}")
        {

        }
    }
}
