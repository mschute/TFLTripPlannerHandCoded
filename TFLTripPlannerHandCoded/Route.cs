namespace TFLTripPlannerHandCoded;

public class Route
{
    public CustomList<RouteFinder.StationLine> Points { get; }
    public int Changes { get; }
    public double Time { get; }

    public Route(CustomList<RouteFinder.StationLine> points, int changes, double time)
    {
        Points = points;
        Changes = changes;
        Time = time;
    }

    public override bool Equals(object obj)
    {
        var route = obj as Route;

        if (null == route)
        {
            return false;
        }

        return Equals(route);
    }

    private bool Equals(Route other)
    {
        if (Changes != other.Changes)
        {
            return false;
        }

        if (Time != other.Time)
        {
            return false;
        }

        if (Points.Count != other.Points.Count)
        {
            return false;
        }

        for (int i = 0; i < Points.Count; i++)
        {
            var station = Points[i];
            var otherStation = other.Points[i];

            if (station.Station.Name != otherStation.Station.Name)
            {
                return false;
            }
        }

        return true;
    }
}