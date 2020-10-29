using System;

namespace Roulette.Exceptions
{
    public class UpdateAmountTooHighException: Exception
    {
        public UpdateAmountTooHighException(Guid id) : base($"Failed to update bet with id {id} as overall amount exceeds maximum amount allowed")
        {

        }
    }
}
