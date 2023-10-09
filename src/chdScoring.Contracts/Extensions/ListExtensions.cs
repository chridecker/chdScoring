using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.Contracts.Extensions
{
    public static class ListExtensions
    {
        public static decimal StandardDeviation(this IEnumerable<decimal> values)
        {
            decimal standardDeviation = 0;

            if (values.Any())
            {
                // Compute the average.     
                var avg = values.Average();

                // Perform the Sum of (value-avg)_2_2.      
                var sum = values.Sum(d => Math.Pow((double)d - (double)avg, 2));

                // Put it all together.      
                standardDeviation = (decimal)Math.Sqrt((sum) / (values.Count() - 1));
            }

            return standardDeviation;
        }
    }
}
