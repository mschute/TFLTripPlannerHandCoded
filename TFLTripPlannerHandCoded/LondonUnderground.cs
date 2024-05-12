namespace TFLTripPlannerHandCoded;

public class LondonUnderground
{
    private readonly IRouteFinder _routeFinder;
    private readonly IStationMap _stationMap;

    public LondonUnderground(IRouteFinder routeFinder, IStationMap stationMap)
    {
        _routeFinder = routeFinder;
        _stationMap = stationMap;
    }

    public void Start()
    {
        var consoleView = new ConsoleView(_stationMap);

        consoleView.OnShortestPath += OnShortestPath;
        consoleView.OnPrintStation += OnPrintStation;
        consoleView.OnTrackSectionOpen += OnTrackSectionOpen;
        consoleView.OnTrackSectionDelay += OnTrackSectionDelay;
        consoleView.OnPrintStationDelays += OnPrintStationDelays;
        consoleView.OnPrintStationClosures += OnPrintStationClosures;

        consoleView.EnterMainMenu();
    }

    private void OnShortestPath(string start, string end)
    {
        var shortestRoute = _routeFinder.FindRoute(_stationMap.Stations, start, end);
        Console.Clear();
        Console.WriteLine(shortestRoute.ToString());
    }

    private void OnPrintStation(string name)
    {
        var station = _stationMap.Stations[name];
        Console.WriteLine("-------------------------------------");
        Console.WriteLine($"Station Details for: {station.Name}");
        Console.WriteLine("Connections:");
        for (var i = 0; i < station.Connections.Count; i++)
        {
            Console.WriteLine($"  - To: {station.Connections[i].DestinationStation.Name}, " +
                              $"Travel time: {station.Connections[i].TravelTime}, " +
                              $"Delay: {station.Connections[i].Delay}, " +
                              $"Open: {station.Connections[i].Open.ToString()}, " +
                              $"Line: {station.Connections[i].Line}, " +
                              $"Direction: {station.Connections[i].Direction}");
        }
    }

    private void OnPrintStationClosures()
    {
        Console.WriteLine("Closed Tracks");
        Console.WriteLine(_stationMap.GetNetworkClosuresStatus());
    }

    private void OnPrintStationDelays()
    {
        Console.WriteLine("Delayed Stations");
        Console.WriteLine(_stationMap.GetNetworkDelaysStatus());
    }

    private void OnTrackSectionDelay(string line, string station, int connection, int delay)
    {
        _stationMap.Connections[line][station][connection].Delay = delay;
    }

    private void OnTrackSectionOpen(string line, string station, int connection, bool open)
    {
        _stationMap.Connections[line][station][connection].Open = open;
    }
}