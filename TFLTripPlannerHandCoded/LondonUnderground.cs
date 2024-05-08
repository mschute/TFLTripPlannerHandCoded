namespace TFLTripPlannerHandCoded;

public class LondonUnderground
{
    //{Station name : Station Object}
    private CustomDictionary<string, Station> _stations = new();

    //{Line name : Connections on that line}
    private CustomDictionary<string, CustomDictionary<string, CustomList<Connection>>> _connections = new();
    public CustomDictionary<string, Station> Stations => _stations;
    public CustomDictionary<string, CustomDictionary<string, CustomList<Connection>>> Connections => _connections;

    public void Start()
    {
        LoadMapData.LoadDataFromCSV("TestData/TestData1.csv", out _stations, out _connections);

        var view = new ConsoleView(this);
    }

    public void HandleUserInput(string response)
    {
        switch (response)
        {
            case "Print Closed Track Sections":
                Console.WriteLine("Closed Tracks");
                Console.WriteLine(GetNetworkStatus("closed"));
                break;

            case "Print Track Section Delays":
                Console.WriteLine("Delayed Stations");
                Console.WriteLine(GetNetworkStatus("delays"));
                break;
        }
    }

    private string GetNetworkStatus(string input)
    {
        string closedTracksMessage = "";
        string delaysMessage = "";

        for (int i = 0; i < _connections.Keys.Count; i++)
        {
            string line = _connections.Keys[i];

            for (int j = 0; j < _connections[line].Keys.Count; j++)
            {
                string station = _connections[line].Keys[j];
                for (int k = 0; k < _connections[line][station].Count; k++)
                {
                    if (!_connections[line][station][k].Open)
                    {
                        closedTracksMessage = closedTracksMessage +
                                              $"Connection from {station} to {_connections[line][station][k].DestinationStation.Name} on {line}\n";
                    }

                    if (_connections[line][station][k].Delay > 0)
                    {
                        delaysMessage = delaysMessage +
                                        $"Connection from {station} to {_connections[line][station][k].DestinationStation.Name} on {line} with Delay {_connections[line][station][k].Delay} mins\n";
                    }
                }
            }
        }

        switch (input)
        {
            case "closed":
                return closedTracksMessage;

            case "delays":
                return delaysMessage;
        }

        return "";
    }

    public void HandleUserInput(string response, CustomList<string> options)
    {
        switch (response)
        {
            case "Calculate Shortest Path":
                var shortestRoute = RouteFinder.FindRoute(_stations, options[0], options[1]);
                // Console.Clear();
                Console.WriteLine(shortestRoute.ToString()); 
                break;

            case "Add Track Section Delay":
                _connections[options[0]][options[1]][int.Parse(options[2])].Delay = int.Parse(options[3]);
                break;

            case "Remove Track Section Delay":
                _connections[options[0]][options[1]][int.Parse(options[2])].Delay = 0;
                break;

            case "Open Track Section":
                _connections[options[0]][options[1]][int.Parse(options[2])].Open = true;
                break;

            case "Close Track Section":
                _connections[options[0]][options[1]][int.Parse(options[2])].Open = false;
                break;

            case "Print Station Information":
                Station station = _stations[options[0]];
                Console.WriteLine("-------------------------------------");
                Console.WriteLine($"Station Details for: {station.Name}");
                Console.WriteLine("Connections:");
                for (int i = 0; i < station.Connections.Count; i++)
                {
                    Console.WriteLine(
                        $"  - To: {station.Connections[i].DestinationStation.Name}, Travel time: {station.Connections[i].TravelTime}, Delay: {station.Connections[i].Delay}, Open: {station.Connections[i].Open.ToString()}, Line: {station.Connections[i].Line}, Direction: {station.Connections[i].Direction}");
                }

                break;
        }
    }

    // private static void PrintShortestPath(Route route)
    // {
    //     Console.Clear();
    //
    //     Console.WriteLine($"Route: {route.Points.First().Station.Name} to {route.Points.Last().Station.Name}:");
    //
    //     Station prev = null;
    //
    //     for (int i = 0; i < route.Points.Count; i++)
    //     {
    //         var station = route.Points[i].Station;
    //         var line = route.Points[i].Line;
    //
    //         if (station != prev)
    //         {
    //             if (station.Previous != null)
    //             {
    //                 Console.WriteLine("(" + (route.Points.IndexOf(route.Points[i]) + 1) + ") " +
    //                                   station.Name + " (Line: " + line + ")" + " " +
    //                                   (station.TimeFromStart - station.Previous.TimeFromStart));
    //             }
    //             else
    //             {
    //                 Console.WriteLine("(" + (route.Points.IndexOf(route.Points[i]) + 1) + ") " +
    //                                   station.Name + " (Line: " + line + ")");
    //             }
    //
    //             prev = station;
    //         }
    //         else
    //         {
    //             Console.WriteLine(line);
    //         }
    //     }
    //
    //     Console.WriteLine($"Total Journey Time: {route.Points.Last().Station.TimeFromStart} minutes");
    //     Console.WriteLine($"Total changes: {route.Changes}");
    // }
}