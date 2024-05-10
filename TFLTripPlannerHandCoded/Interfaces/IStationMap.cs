namespace TFLTripPlannerHandCoded;

public interface IStationMap
{
    CustomDictionary<string, Station> Stations { get; }
    CustomDictionary<string, CustomDictionary<string, CustomList<Connection>>> Connections { get; }

    string GetNetworkClosuresStatus();
    string GetNetworkDelaysStatus();
}