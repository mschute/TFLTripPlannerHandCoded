namespace TFLTripPlannerHandCoded;

public class RouteNode
{
    public string StationName { get; }
    public string Line { get; }
    public double TimeFromPrevious { get; }
    public string Direction { get; }

    public RouteNode(string station, string line, double timeFromPrevious, string direction)
    {
        StationName = station;
        Line = line;
        TimeFromPrevious = timeFromPrevious;
        Direction = direction;
    }
}