using System;

namespace Roulette.Exceptions
{
    public class FailedToModifyException : Exception
    {
        public FailedToModifyException(string message) : base(message)
        {

        }
    }
}
