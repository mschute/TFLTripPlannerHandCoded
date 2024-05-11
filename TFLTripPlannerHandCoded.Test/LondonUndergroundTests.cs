namespace TFLTripPlannerHandCoded.Test;

public class LondonUndergroundTests
{
    [Test]
    public void TestAToE()
    {
        LoadMapData.LoadDataFromCSV("TestData/TestData1.csv", out var stations, out var connections);
        var routeFinder = new RouteFinder();
        
        var points = new CustomList<RouteNode>();
        points.Add(new RouteNode("STATION A", "GREEN", 0, "NA"));
        points.Add(new RouteNode("STATION B", "GREEN", 3, "NA"));
        points.Add(new RouteNode("STATION D", "GREEN", 5, "NA"));
        points.Add(new RouteNode("STATION E", "GREEN", 2, "NA"));
        
        var expected = new Route(points, 0, 10.0);
        
        // Act
        var actual = routeFinder.FindRoute(stations, "STATION A", "STATION E");
        
        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    public void TestEToA()
    {
        LoadMapData.LoadDataFromCSV("TestData/TestData1.csv", out var stations, out var connections);
        var routeFinder = new RouteFinder();
        
        var points = new CustomList<RouteNode>();
        points.Add(new RouteNode("STATION E", "GREEN", 0, "NA"));
        points.Add(new RouteNode("STATION D", "GREEN", 2, "NA"));
        points.Add(new RouteNode("STATION B", "GREEN", 5, "NA"));
        points.Add(new RouteNode("STATION A", "GREEN", 3, "NA"));
        
        var expected = new Route(points, 0, 10.0);
        
        // Act
        var actual = routeFinder.FindRoute(stations, "STATION E", "STATION A");
        
        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    public void TestAToC()
    {
        LoadMapData.LoadDataFromCSV("TestData/TestData1.csv", out var stations, out var connections);
        var routeFinder = new RouteFinder();
        
        var points = new CustomList<RouteNode>();
        points.Add(new RouteNode("STATION A", "GREEN", 0, "NA"));
        points.Add(new RouteNode("STATION B", "GREEN", 3, "NA"));
        points.Add(new RouteNode("STATION C", "YELLOW", 4, "NA"));
        
        var expected = new Route(points, 1, 9.0);
        
        // Act
        var actual = routeFinder.FindRoute(stations, "STATION A", "STATION C");
        
        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }
}