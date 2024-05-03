namespace TFLTripPlannerHandCoded.Test;

public class LondonUndergroundTests
{
    [Test]
    public void TestAToE()
    {
        LoadMapData.LoadDataFromCSV("TestData/TestData1.csv", out var stations, out var connections);
        
        var points = new CustomList<RouteFinder.StationLine>();
        points.Add(new RouteFinder.StationLine(stations["STATION A"], ""));
        points.Add(new RouteFinder.StationLine(stations["STATION B"], ""));
        points.Add(new RouteFinder.StationLine(stations["STATION D"], ""));
        points.Add(new RouteFinder.StationLine(stations["STATION E"], ""));
        
        var expected = new Route(points, 0, 10.0);
        
        // Act
        var actual = RouteFinder.findRoute(stations, "STATION A", "STATION E");
        
        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    public void TestEToA()
    {
        LoadMapData.LoadDataFromCSV("TestData/TestData1.csv", out var stations, out var connections);
        
        var points = new CustomList<RouteFinder.StationLine>();
        points.Add(new RouteFinder.StationLine(stations["STATION E"], ""));
        points.Add(new RouteFinder.StationLine(stations["STATION D"], ""));
        points.Add(new RouteFinder.StationLine(stations["STATION B"], ""));
        points.Add(new RouteFinder.StationLine(stations["STATION A"], ""));
        
        var expected = new Route(points, 0, 10.0);
        
        // Act
        var actual = RouteFinder.findRoute(stations, "STATION E", "STATION A");
        
        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }
}