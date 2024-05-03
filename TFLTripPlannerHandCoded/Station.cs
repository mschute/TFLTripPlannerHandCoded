namespace TFLTripPlannerHandCoded;

public class Station : IComparer<Station>
{
    public string Name { get; }
    public CustomList<Connection> Connections { get; }

    // Should the following class fields be stored here?
    // They are only used for route finding
    public double TimeFromStart { get; set; }
    public bool Visited { get; set; }
    public Station Previous { get; set; }
    public Station Next { get; set; }
    public string CurrentLine { get; set; }

    public Station(string name)
    {
        Name = name;
        Connections = new CustomList<Connection>();
        TimeFromStart = double.PositiveInfinity;
        Visited = false;
        Previous = null;
        Next = null;
        CurrentLine = "";
    }

    public void AddConnection(Station station, double weight, string line, string direction)
    {
        Connections.Add(new Connection(station, weight, line, direction));
    }

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