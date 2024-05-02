namespace TFLTripPlannerHandCoded;

public class Connection
{
    public Station DestinationStation { get; }
    public double TravelTime { get; set; }
    public double Delay { get; set; }
    public string Line { get; } //Ex: Northern, Circle....
    public bool Open { get; set; }
    public string Direction { get; } //Ex: Northbound, Southbound.......


    public Connection(Station destinationStation, double travelTime, string line, string direction)
    {
        DestinationStation = destinationStation;
        TravelTime = travelTime;
        Delay = 0;
        Line = line;
        Open = true;
        Direction = direction;
    }
}