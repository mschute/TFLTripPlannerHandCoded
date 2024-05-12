namespace TFLTripPlannerHandCoded;

public class StationMapModel : IStationMap
{
    private CustomDictionary<string, Station> _stations = new();
    private CustomDictionary<string, CustomDictionary<string, CustomList<Connection>>> _connections = new();

    public CustomDictionary<string, Station> Stations => _stations;
    public CustomDictionary<string, CustomDictionary<string, CustomList<Connection>>> Connections => _connections;

    private string modelFilePath;

    public StationMapModel(string modelFilePath)
    {
        this.modelFilePath = modelFilePath;

        LoadData();
    }

    private void LoadData()
    {
        LoadMapData.LoadDataFromCSV(modelFilePath, out _stations, out _connections);
    }

    public string GetNetworkClosuresStatus()
    {
        var status = "";
        for (var i = 0; i < Connections.Keys.Count; i++)
        {
            var line = Connections.Keys[i];

            for (var j = 0; j < Connections[line].Keys.Count; j++)
            {
                var station = Connections[line].Keys[j];
                for (var k = 0; k < Connections[line][station].Count; k++)
                {
                    if (!Connections[line][station][k].Open)
                    {
                        status += $"Connection from {station} " +
                                  $"to {Connections[line][station][k].DestinationStation.Name} " +
                                  $"on {line}\n";
                    }
                }
            }
        }

        return status;
    }

    public string GetNetworkDelaysStatus()
    {
        var status = "";
        for (var i = 0; i < Connections.Keys.Count; i++)
        {
            var line = Connections.Keys[i];

            for (var j = 0; j < Connections[line].Keys.Count; j++)
            {
                var station = Connections[line].Keys[j];
                for (var k = 0; k < Connections[line][station].Count; k++)
                {
                    if (Connections[line][station][k].Delay > 0)
                    {
                        status += $"Connection from {station} " +
                                  $"to {Connections[line][station][k].DestinationStation.Name} " +
                                  $"on {line} " +
                                  $"with Delay {Connections[line][station][k].Delay} mins\n";
                    }
                }
            }
        }

        return status;
    }
}