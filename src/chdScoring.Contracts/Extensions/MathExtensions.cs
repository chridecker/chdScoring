using System;
using System.Collections.Generic;
using System.Text;

namespace chdScoring.Contracts.Extensions
{
    public static class MathExtensions
    {
        public static decimal RoundToNearestHalf(this decimal value)
        {
            return Math.Round(value * 2, MidpointRounding.AwayFromZero) / 2;
        }
    }
}
