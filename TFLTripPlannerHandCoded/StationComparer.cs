using static TFLTripPlannerHandCoded.RouteFinder;

namespace TFLTripPlannerHandCoded
{
    public class StationComparer : IComparer<RouteNode>
    {
        public int Compare(RouteNode x, RouteNode y)
        {
            // Compare stations by name first
            int nameComparison = x.Station.Name.CompareTo(y.Station.Name);
            if (nameComparison != 0)
            {
                return nameComparison;
            }

            // If names are equal, compare by time from start
            return x.Station.TimeFromStart.CompareTo(y.Station.TimeFromStart);
        }
    }
}