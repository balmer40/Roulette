using System;

namespace Roulette.Exceptions
{
    public class FailedToUpdateGameException : FailedToModifyException
    {
        public FailedToUpdateGameException(Guid id) : base($"Failed to update game with id: {id}")
        {

        }
    }
}
