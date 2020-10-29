using System;

namespace Roulette.Exceptions
{
    public class FailedToUpdateBetException : FailedToModifyException
    {
        public FailedToUpdateBetException(Guid id) : base($"Failed to update bet with id: {id}")
        {

        }
    }
}
