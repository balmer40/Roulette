using System;

namespace Roulette.Exceptions
{
    public class GameNotFoundException : NotFoundException
    {
        public GameNotFoundException(Guid id) : base($"Game not found with id: {id}")
        {

        }
    }
}
