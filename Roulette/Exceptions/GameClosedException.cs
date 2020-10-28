using System;

namespace Roulette.Exceptions
{
    public class GameClosedException : Exception
    {
        public GameClosedException(Guid id) : base($"Game with id {id} is closed")
        {

        }
    }
}
