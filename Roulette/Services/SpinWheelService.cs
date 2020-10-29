using System;

namespace Roulette.Services
{
    public class SpinWheelService : ISpinWheelService
    {
        private readonly Random _random = new Random();
        private int MAXIMUM_WINNING_NUMBER = 36;

        public int GetWinningNumber()
        {
            return _random.Next(MAXIMUM_WINNING_NUMBER);
        }
    }
}
