using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Roulette.Models
{
    public class RouletteBoard
    {
        public int SpinWheel()
        {
            // TODO random between 0-38/39
            return new Random().Next();
        }
    }
}
