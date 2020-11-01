namespace Roulette.Constants
{
    public class RegularExpressions
    {
        public const string Amount = @"\d+(\.\d{1,2})?";

        private const string Guid = @"^[A-Za-z0-9]{8}(-[A-Za-z0-9]{4}){3}-[A-Za-z0-9]{12}$";

        // Look ahead to see that the string contains at least one character that's not 0 or dash,
        // and then try to match the GUID regex.
        public const string NonEmptyGuid = "(?=.*[^0-])" + Guid;
    }
}
