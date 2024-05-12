namespace TFLTripPlannerHandCoded;

public class RouteFinder : IRouteFinder
{
    // Structure to store all the calculation related information for a station
    private class CalcNode : IComparable<CalcNode>
    {
        public Station Station { get; }
        public string Direction { get; set; }
        public string CurrentLine { get; set; }
        public double TimeFromStart { get; set; }

        public double TimeFromPrevious { get; set; }
        public CalcNode Previous { get; set; }
        public CalcNode Next { get; set; }
        public bool Visited { get; set; }

        public CalcNode(Station station)
        {
            Station = station;
            Direction = "";
            CurrentLine = "";
            TimeFromStart = double.PositiveInfinity;
            TimeFromPrevious = 0.0;
            Previous = null;
            Next = null;
            Visited = false;
        }

        public int CompareTo(CalcNode? other)
        {
            if (other == default)
            {
                return 1;
            }

            var compare = TimeFromStart.CompareTo(other.TimeFromStart);

            if (compare == 0)
            {
                return Station.Name.CompareTo(other.Station.Name);
            }

            return compare;
        }
    }

    public Route FindRoute(CustomDictionary<string, Station> stations, string startName, string endName)
    {
        if (!stations.ContainsKey(startName) || !stations.ContainsKey(endName))
        {
            return null;
        }

        var stationNames = stations.Keys;
        var calcNodes = new CustomDictionary<string, CalcNode>();
        for (var i = 0; i < stationNames.Count; i++)
        {
            calcNodes[stationNames[i]] = new CalcNode(stations[stationNames[i]]);
        }

        //Assign start station
        var startNode = calcNodes[startName];
        startNode.TimeFromStart = 0;

        var unexploredNodes = new HandCodedMinHeap<CalcNode>(1);
        unexploredNodes.Insert(startNode);

        List<string> visited = new List<string>();

        // Score all the nodes, recording the best times and transitions at each node
        while (unexploredNodes.Size > 0)
        {
            //Current station is the one at the front of the queue
            var currentNode = unexploredNodes.Root;
            visited.Add(currentNode.Station.Name);
            var currentConnections = currentNode.Station.Connections;

            for (var i = 0; i < currentConnections.Count; i++)
            {
                if (currentConnections[i].Open)
                {
                    var penalty = 0;
                    var destinationStation = currentConnections[i].DestinationStation;
                    var neighborNode = calcNodes[destinationStation.Name];

                    if (currentNode.CurrentLine != currentConnections[i].Line && currentNode.CurrentLine != "")
                    {
                        penalty = 2;
                    }

                    var tentativeTravelTime = currentNode.TimeFromStart
                                              + currentConnections[i].Delay
                                              + currentConnections[i].TravelTime
                                              + penalty;

                    // Only record the best time at each node
                    if (tentativeTravelTime < neighborNode.TimeFromStart)
                    {
                        neighborNode.TimeFromStart = tentativeTravelTime;
                        neighborNode.Previous = currentNode;
                        neighborNode.CurrentLine = currentConnections[i].Line;
                        neighborNode.Direction = currentConnections[i].Direction;
                        neighborNode.TimeFromPrevious = currentConnections[i].TravelTime;
                    }

                    if (neighborNode.Visited == false && !unexploredNodes.Has(neighborNode))
                    {
                        unexploredNodes.Insert(neighborNode);
                    }
                }
            }

            unexploredNodes.Delete(currentNode);
            currentNode.Visited = true;
        }

        string visitedStations = string.Join(", ", visited);
        Console.WriteLine("Sequence of explored stations: " + visitedStations);

        // Build path from end to start based on the attributes of each calc node
        var current = calcNodes[endName];
        var next = current.Previous;

        var changes = 0;
        var totalTime = current.TimeFromStart;
        var shortestPath = new CustomList<RouteNode>();

        shortestPath.Add(new RouteNode(current.Station.Name, current.CurrentLine, current.TimeFromPrevious,
            current.Direction));

        while (next != null)
        {
            if (next.CurrentLine != current.CurrentLine)
            {
                if (string.IsNullOrEmpty(next.CurrentLine))
                {
                    next.CurrentLine = current.CurrentLine;
                    next.Direction = current.Direction;
                }
                else
                {
                    changes++;
                }
            }

            shortestPath.Insert(0,
                new RouteNode(next.Station.Name, next.CurrentLine, next.TimeFromPrevious, next.Direction));
            current = next;
            next = current.Station == stations[startName] ? null : current.Previous;
        }

        return new Route(shortestPath, changes, totalTime);
    }
}