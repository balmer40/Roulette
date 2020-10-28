using System;

namespace Roulette.Exceptions
{
    public class GameStatusException : Exception
    {
        public GameStatusException(string message) : base(message)
        {

        }
    }
}
