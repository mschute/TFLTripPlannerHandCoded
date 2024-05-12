namespace TFLTripPlannerHandCoded;

using Stations = CustomDictionary<string, Station>;
using Connections = CustomDictionary<string, CustomDictionary<string, CustomList<Connection>>>;

public static class LoadMapData
{
    public static void LoadDataFromCSV(string filePath, out Stations stations, out Connections connections)
    {
        stations = new Stations();
        connections = new Connections();

        using (var reader = new StreamReader(filePath))
        {
            reader.ReadLine();

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');

                var lineName = values[0];
                var direction = values[1];
                var stationA = values[2];
                var stationB = values[3];
                double distanceKM, unImpededTime;
                if (!double.TryParse(values[4], out distanceKM) || !double.TryParse(values[5], out unImpededTime))
                {
                    continue;
                }

                var trainLine = lineName;

                // Add stations and connections
                if (!stations.ContainsKey(stationA))
                {
                    stations[stationA] = new Station(stationA);
                }

                if (!stations.ContainsKey(stationB))
                {
                    stations[stationB] = new Station(stationB);
                }

                if (!connections.ContainsKey(trainLine))
                {
                    connections[trainLine] = new CustomDictionary<string, CustomList<Connection>>();
                }

                // Using un-impeded running time as weight
                var travelTime = unImpededTime;

                AddConnection(stations, connections, stationA, stationB, travelTime, trainLine, direction);
            }
        }
    }

    private static void AddConnection(Stations stations, Connections connections, string station1, string station2,
        double travelTime, string line,
        string direction)
    {
        if (!stations.ContainsKey(station1) || !stations.ContainsKey(station2))
        {
            throw new ArgumentException("One or both of the stations do not exist.");
        }

        stations[station1].AddConnection(stations[station2], travelTime, line, direction);

        //Adding all connections for station1 in at the given line to the Connections list
        if (connections[line].ContainsKey(station1))
        {
            // If the inner key exists, add the item to the list associated with that inner key
            connections[line][station1].Add(stations[station1].Connections.Last());
        }
        else
        {
            // If the inner key does not exist, create a new list and add the item to it
            var newCustomList = new CustomList<Connection>();
            newCustomList.Add(stations[station1].Connections.Last());

            // Add the inner key along with the list to the inner dictionary
            connections[line].Add(station1, newCustomList);
        }
    }
}