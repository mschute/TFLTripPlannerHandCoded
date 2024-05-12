namespace TFLTripPlannerHandCoded;

class Program
{
    static void Main(string[] args)
    {
        var routeFinder = new RouteFinder();
        var stationMapModel = new StationMapModel("TestData/TestData1.csv");

        var londonUnderground = new LondonUnderground(routeFinder, stationMapModel);

        londonUnderground.Start();
    }
}