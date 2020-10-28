using System;

namespace Roulette.Exceptions
{
    public class FailedToDeleteBetException : FailedToModifyException
    {
        public FailedToDeleteBetException(Guid id) : base($"Failed to delete bet with id: {id}")
        {

        }
    }
}
