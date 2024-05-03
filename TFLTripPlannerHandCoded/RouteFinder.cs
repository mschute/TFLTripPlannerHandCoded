namespace TFLTripPlannerHandCoded;

public static class RouteFinder
{
    // Should we be using a structure like this to associate the Station and the related node info
    private class RouteNode
    {
        public Station Station { get; }

        public double TimeFromStart { get; set; }
        public bool Visited { get; set; }
        public Station Previous { get; set; }
        public Station Next { get; set; }
        public string CurrentLine { get; set; }

        public RouteNode(Station station)
        {
            Station = station;
        }
    }
    
    public class StationLine
    {
        public Station Station { get; }
        public string Line { get; }

        public StationLine(Station station, string line)
        {
            Station = station;
            Line = line;
        }
    }
    
    public static Route findRoute(CustomDictionary<string, Station> stations, string startName, string endName)
    {
        // TODO If/when we switch to the RouteNode class, we should copy the stations over for calculations
        // CustomDictionary<string, RouteNode> stationNodes = new CustomDictionary<string, RouteNode>();
        //
        // var stationNames = stations.Keys;
        // for (int i = 0; i < stationNames.Count; i++)
        // {
        //     stationNodes[stationNames[i]] = new RouteNode(stations[stationNames[i]]);
        // }
        
        //TODO this is because the previous implementation was clearing the route related data from the stations
        // The Stations should not contain any route calculation data and the stationNodes should be used instead
        var keys = stations.Keys;
        for (var i = 0; i < keys.Count; i++)
        {
            var station = stations.GetValue(keys[i]);
            station.TimeFromStart = double.PositiveInfinity;
            station.Visited = false;
            station.Previous = null;
            station.Next = null;
            station.CurrentLine = "";
        }
        
        int changes = 0;

        //Assign start station
        var startStation = stations[startName];
        startStation.TimeFromStart = 0;

        //TODO SortedSet is a generic collection so we can't use it in this project
        //Comparer object passed into sorted set, and ordering the stations in this sortedSet by time from start
        var unexploredStations = new SortedSet<Station>(new StationComparer());
        unexploredStations.Add(startStation);

        //Finding the shortest path from start to end

        //While there are stations to be explored
        while (unexploredStations.Count > 0)
        {
            //Current station is the one at the front of the queue (i.e unexploredStations.min)
            var currentStation = unexploredStations.Min;
            
            for (int i = 0; i < currentStation.Connections.Count; i++)
            {
                if (currentStation.Connections[i].Open)
                {
                    int change = 0;
                    var neighborStation = currentStation.Connections[i].DestinationStation;
                    if (currentStation.CurrentLine != currentStation.Connections[i].Line &&
                        currentStation.CurrentLine != "")
                    {
                        change = 2;
                    }

                    var tentativeTravelTime = currentStation.TimeFromStart + currentStation.Connections[i].Delay +
                                              currentStation.Connections[i].TravelTime + change;


                    //Compare current neighbouring stations time from start with the current stations time from start, plus connection weight
                    //to neighbouring station
                    if (tentativeTravelTime < neighborStation.TimeFromStart)
                    {
                        neighborStation.TimeFromStart = tentativeTravelTime;
                        neighborStation.Previous = currentStation;
                        neighborStation.CurrentLine = currentStation.Connections[i].Line;

                        if (!unexploredStations.Contains(neighborStation))
                        {
                            unexploredStations.Add(neighborStation);
                        }
                    }
                }
            }

            //After current station has been explored, remove it from the front of the queue    
            unexploredStations.Remove(currentStation);
            currentStation.Visited = true;
        }

        //A list of tuples, current station (Station) and a string holding, previous station and line taken (String)
        var shortestPath = new CustomList<StationLine>();
        Station next = null;
        var current = stations[endName];
        var currentLine = "";
        Connection previousConnection = null;
        Connection nextConnection = null;

        //Build path from end to start based on the .Previous attributes of each station
        while (current != null)
        {
            if (current.Previous != null)
            {
                current.Next = next;
                //The previous connection is found by finding the previous station, whos connection = our current station (and is open)
                previousConnection =
                    current.Previous.Connections.FirstOrDefault(c =>
                        c.DestinationStation == current && c.Open != false);
                if (current != stations[endName])
                {
                    nextConnection = current.Connections.FirstOrDefault(c =>
                        c.DestinationStation.Name == current.Next.Name & c.Open != false);
                }

                if (previousConnection != null)
                {
                    //If the line or direction changes, update the current line, and add line change message
                    if ($"{previousConnection.Line} {previousConnection.Direction}" != currentLine)
                    {
                        if (currentLine != "")
                        {
                            var lineChangeMessage = "";
                            if (next != null)
                            {
                                changes += 2;
                                //shortestPath.Last().Item1.TimeFromStart += 2;
                                lineChangeMessage =
                                    $"Change: {string.Join(",", previousConnection.Line)} {previousConnection.Direction} to {nextConnection.Line} ({nextConnection.Direction}) 2.00 mins";
                            }

                            shortestPath.Insert(0, new StationLine(current, lineChangeMessage));
                        }

                        currentLine = $"{previousConnection.Line} {previousConnection.Direction}";
                    }
                }
            }

            next = current;
            shortestPath.Insert(0, new StationLine(current, currentLine));
            current = current.Previous;
        }

        return new Route(shortestPath, changes, shortestPath.Last().Station.TimeFromStart);
    }
}