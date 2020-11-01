using Roulette.Models;

namespace Roulette.Exceptions
{
    public class BetAlreadyExistsException : BadRequestBetException
    {
        public BetAlreadyExistsException(BetType betType, int? position) : base($"Bet with '{betType}' and '{position}' already exists for customer so cannot be created")
        {

        }
    }
}
