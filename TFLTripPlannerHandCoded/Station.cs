namespace TFLTripPlannerHandCoded;

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
}