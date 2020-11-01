using System;

namespace Roulette.Exceptions
{
    public class FailedToCreateBetException : FailedToModifyException
    {
        public FailedToCreateBetException(Guid id) : base($"Failed to create bet for game with id: {id}")
        {

        }
    }
}
