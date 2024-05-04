namespace TFLTripPlannerHandCoded;

public static class RouteFinder
{
    // Should we be using a structure like this to associate the Station and the related node info
    public class RouteNode
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
        // public string Direction { get; set; }

        public StationLine(Station station, string line)
        {
            Station = station;
            Line = line;
            // Direction = direction;
        }
    }
    
    public static Route findRoute(CustomDictionary<string, Station> stations, string startName, string endName)
    {
        // TODO If/when we switch to the RouteNode class, we should copy the stations over for calculations
        CustomDictionary<string, RouteNode> stationNodes = new CustomDictionary<string, RouteNode>();
        
        var stationNames = stations.Keys;
        for (int i = 0; i < stationNames.Count; i++)
        {
            stationNodes[stationNames[i]] = new RouteNode(stations[stationNames[i]]);
        }
        
        //TODO this is because the previous implementation was clearing the route related data from the stations
        // The Stations should not contain any route calculation data and the stationNodes should be used instead
        // var keys = stations.Keys;
        // for (var i = 0; i < keys.Count; i++)
        // {
        //     var station = stations.GetValue(keys[i]);
        //     station.TimeFromStart = double.PositiveInfinity;
        //     station.Visited = false;
        //     station.Previous = null;
        //     station.Next = null;
        //     station.CurrentLine = "";
        // }

        var keys = stationNodes.Keys;
        for (var i = 0; i < keys.Count; i++)
        {
            var station = stationNodes.GetValue(keys[i]);
            station.TimeFromStart = double.PositiveInfinity;
            station.Visited = false;
            station.Previous = null;
            station.Next = null;
            station.CurrentLine = "";
        }
        
        int changes = 0;

        //Assign start station
        var startRouteNode = stationNodes[startName];
        startRouteNode.TimeFromStart = 0;

        //TODO SortedSet is a generic collection so we can't use it in this project
        //Comparer object passed into sorted set, and ordering the stations in this sortedSet by time from start
        var unexploredNodes = new SortedSet<RouteNode>(new StationComparer());
        unexploredNodes.Add(startRouteNode);

        //Finding the shortest path from start to end

        //While there are stations to be explored
        while (unexploredNodes.Count > 0)
        {
            //Current station is the one at the front of the queue (i.e unexploredStations.min)
            var currentNode = unexploredNodes.Min;

            for (int i = 0; i < currentNode.Station.Connections.Count; i++)
            {
                if (currentNode.Station.Connections[i].Open)
                {
                    int change = 0;
                    var destinationStation = currentNode.Station.Connections[i].DestinationStation;
                    var neighborNode = stationNodes[destinationStation.Name];

                    if (currentNode.CurrentLine != currentNode.Station.Connections[i].Line &&
                        currentNode.CurrentLine != "")
                    {
                        change = 2;
                    }

                    var tentativeTravelTime = currentNode.TimeFromStart + currentNode.Station.Connections[i].Delay +
                    currentNode.Station.Connections[i].TravelTime + change;


                    //Compare current neighbouring stations time from start with the current stations time from start, plus connection weight
                    //to neighbouring station
                    if (tentativeTravelTime < neighborNode.TimeFromStart)
                    {
                        neighborNode.TimeFromStart = tentativeTravelTime;
                        neighborNode.Previous = currentNode.Station;
                        neighborNode.CurrentLine = currentNode.Station.Connections[i].Line;

                        if (!unexploredNodes.Contains(neighborNode))
                        {
                            unexploredNodes.Add(neighborNode);
                        }
                    }
                }
            }

            //After current station has been explored, remove it from the front of the queue    
            unexploredNodes.Remove(currentNode);
            currentNode.Visited = true;
        }
    
        //TODO I believe its integrating this bit that has caused the issues

        //A list of tuples, current station (Station) and a string holding, previous station and line taken (String)
        var shortestPath = new CustomList<StationLine>();
        RouteNode next = null;
        var current = stationNodes[endName];
        var currentLine = "";
        StationLine previousConnection = null;
        StationLine nextConnection = null;

        //Build path from end to start based on the .Previous attributes of each station
        while (current != null)
        {
            if (current.Previous != null)
            {

                current.Station.Next = next.Station;
                //The previous connection is found by finding the previous station, whos connection = our current station (and is open)
                  previousConnection =
                new StationLine(current.Previous, current.Previous.CurrentLine);

                // previousConnection =
                //     current.Previous.Connections.FirstOrDefault(c =>
                //         c.DestinationStation == current.Station && c.Open != false);
                if (current.Station != stationNodes[endName].Station)
                {
                    nextConnection = new StationLine(current.Station, current.CurrentLine);
                }

                if (previousConnection != null)
                {
                    //If the line or direction changes, update the current line, and add line change message
                    if ($"{previousConnection.Line}" != currentLine)
                    {
                        if (currentLine != "")
                        {
                            var lineChangeMessage = "";
                            if (next != null)
                            {
                                changes += 2;
                                //shortestPath.Last().Item1.TimeFromStart += 2;
                                lineChangeMessage =
                                    $"Change: {string.Join(",", previousConnection.Line)} to {nextConnection.Line} () 2.00 mins";
                            }

                            shortestPath.Insert(0, new StationLine(current.Station, lineChangeMessage));
                        }

                        currentLine = $"{previousConnection.Line}";
                    }
                }
            }

            next = current;
            shortestPath.Insert(0, new StationLine(current.Station, currentLine));
            var previousStation = current.Previous;
            current = stationNodes[previousStation.Name];
            //current = new RouteNode(previousStation);
            //current = current.Previous;
        }

        return new Route(shortestPath, changes, shortestPath.Last().Station.TimeFromStart);
    }
}