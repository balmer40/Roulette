namespace Roulette.Exceptions
{
    public class FailedToCreateGameException : FailedToModifyException
    {
        public FailedToCreateGameException() : base("Failed to create game")
        {

        }
    }
}
