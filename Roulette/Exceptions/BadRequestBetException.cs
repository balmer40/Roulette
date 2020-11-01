using System;

namespace Roulette.Exceptions
{
    public class BadRequestBetException : Exception
    {
        public BadRequestBetException(string message) : base(message) { }
    }
}
