namespace TFLTripPlannerHandCoded;

public interface IRouteFinder
{
    Route FindRoute(CustomDictionary<string, Station> stations, string startName, string endName);
}