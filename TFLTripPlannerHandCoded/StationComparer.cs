using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TFLTripPlannerHandCoded
{
   public class StationComparer : IComparer<Station>
{
    public int Compare(Station x, Station y)
    {
        // Compare stations by name first
        int nameComparison = x.Name.CompareTo(y.Name);
        if (nameComparison != 0)
        {
            return nameComparison;
        }

        // If names are equal, compare by time from start
        return x.TimeFromStart.CompareTo(y.TimeFromStart);
    }
}
}