using System;

namespace Roulette.Exceptions
{
    public class BetNotFoundException : NotFoundException
    {
        public BetNotFoundException(Guid id) : base($"Bet not found with id: {id}")
        {

        }
    }
}
