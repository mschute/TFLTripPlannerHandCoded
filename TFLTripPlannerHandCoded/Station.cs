namespace TFLTripPlannerHandCoded;

//public class Station : IComparer<Station>
public class Station
{
    public string Name { get; }
    public CustomList<Connection> Connections { get; }

    public Station(string name)
    {
        Name = name;
        Connections = new CustomList<Connection>();
    }

    public void AddConnection(Station station, double weight, string line, string direction)
    {
        Connections.Add(new Connection(station, weight, line, direction));
    }

    //TODO Do we need compare in here anymore?
    // public int Compare(Station x, Station y)
    // {
    //     // Compare stations by name first
    //     var nameComparison = x.Name.CompareTo(y.Name);
    //     if (nameComparison != 0)
    //     {
    //         return nameComparison;
    //     }
    //
    //     // If names are equal, compare by time from start
    //     return x.TimeFromStart.CompareTo(y.TimeFromStart);
    // }
}