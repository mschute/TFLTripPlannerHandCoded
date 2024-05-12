using System.Text;

namespace TFLTripPlannerHandCoded;

public class Route
{
    public CustomList<RouteNode> Points { get; }
    public int Changes { get; }
    public double TotalTime { get; }

    public TextWriter writer = new TextWriter();

    public Route(CustomList<RouteNode> points, int changes, double totalTime)
    {
        Points = points;
        Changes = changes;
        TotalTime = totalTime;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Points, Changes, TotalTime);
    }

    public override bool Equals(object? obj) => Equals(obj as Route);

    private bool Equals(Route? other)
    {
        if (other == null)
        {
            return false;
        }

        if (Changes != other.Changes)
        {
            return false;
        }

        if (Math.Abs(TotalTime - other.TotalTime) > 1e-06)
        {
            return false;
        }

        if (Points.Count != other.Points.Count)
        {
            return false;
        }

        for (var i = 0; i < Points.Count; i++)
        {
            var station = Points[i];
            var otherStation = other.Points[i];

            if (station.StationName != otherStation.StationName)
            {
                return false;
            }

            if (station.Direction != otherStation.Direction)
            {
                return false;
            }

            if (Math.Abs(station.TimeFromPrevious - otherStation.TimeFromPrevious) > 1e-6)
            {
                return false;
            }

            if (station.Line != otherStation.Line)
            {
                return false;
            }
        }

        return true;
    }

    public override string ToString()
    {
        var result = new StringBuilder();
        var start = Points.First();
        var routeOutputNo = 2;
        File.Delete("V1_output.txt");

        result.AppendLine($"(1) Start: {start.StationName}, {start.Line} ({start.Direction})");

        for (var i = 0; i < Points.Count - 1; i++)
        {
            var current = Points[i];
            var next = Points[i + 1];

            if (current.Line != next.Line)
            {
                result.AppendLine(
                    $"({routeOutputNo}) Change: {current.StationName} {current.Line} ({current.Direction}) to {next.Line} ({next.Direction}) {2:#.00}min");
                routeOutputNo++;
            }

            if (next != null)
            {
                result.AppendLine(
                    $"({routeOutputNo}) {current.Line} ({current.Direction}): {current.StationName} to {next.StationName}: {next.TimeFromPrevious:#.00}min");
                routeOutputNo++;
            }
            else
            {
                result.AppendLine(
                    $"({routeOutputNo}) End: {current.StationName}, {current.Line} ({current.Direction})");
            }
        }

        result.AppendLine($"Total Journey Time: {TotalTime:#.00} minutes");
        writer.WriteToFile(result.ToString());

        return result.ToString();
    }
}