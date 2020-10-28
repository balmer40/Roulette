namespace Roulette.Exceptions
{
    public class FailedToCreateBetException : FailedToModifyException
    {
        public FailedToCreateBetException() : base("Failed to create bet")
        {

        }
    }
}
