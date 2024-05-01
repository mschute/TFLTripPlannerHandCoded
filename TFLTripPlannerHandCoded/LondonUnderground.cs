// using TFLTripPlannerHandCoded;
//
// namespace TFLTripPlanner;
//
// public static class LondonUnderground
// {
//     //{Station name : Station Object}
//     public static CustomDictionary<string, Station> Stations { get; set; }
//     //{Line name : Connections on that line}
//     public static CustomDictionary<string, CustomDictionary<string ,List<Connection>>> Connections { get; set; }
//
//     public static void Start()
//     {
//         Stations = new CustomDictionary<string, Station>();
//         Connections = new CustomDictionary<string, CustomDictionary<string, List<Connection>>>();
//         LoadDataFromCSV("TestData/TestData1.csv");
//         
//         
//         //// //Print Graph
//         // Console.WriteLine("Stations and Connections:");
//         // foreach (var station in Stations.Values)
//         // {
//         //     Console.WriteLine($"Station: {station.Name}");
//         //     foreach (var connection in station.Connections)
//         //     {
//         //         Console.WriteLine($"  - To: {connection.DestinationStation.Name}, Travel time: {connection.TravelTime}, Line: {connection.Line}");
//         //     }
//         // }
//         
//         ////Print Connections
//         // Console.WriteLine("Stations and Connections:");
//         // foreach (var Line in Connections.Keys)
//         // {
//         //     Console.WriteLine($"Line: {Line}");
//         //     foreach (var connection in Connections[Line])
//         //     {
//         //         foreach (var s in connection.Value)
//         //         {
//         //             Console.WriteLine($"  - From: {connection.Key} To: {s.DestinationStation.Name}, Weight: {s.TravelTime}, Delay: {s.Delay}, Open: {s.Open.ToString()}, Line: {s.Line}");
//         //         }
//         //     }
//         // }
//         
//         //Organising Stations Dict in alphabetical order.
//         //Stations = Stations.OrderBy(x => x.Key).ToCustomDictionary(x => x.Key, x => x.Value);
//         
//         ConsoleView view = new ConsoleView();
//     }
//     
//     public static void HandleUserInput(string response)
//     {
//         switch (response)
//         {
//             case "Print Closed Track Sections":
//                 Console.WriteLine("Closed Tracks");
//                 Console.WriteLine(GetNetworkStatus("closed"));
//                 break;
//
//             case "Print Track Section Delays":
//                 Console.WriteLine("Delayed Stations");
//                 Console.WriteLine(GetNetworkStatus("delays"));
//                 break;
//         }
//     }
//
//     private static string GetNetworkStatus(string input)
//     {
//         string closedTracksMessage = "";
//         string delaysMessage = "";
//
//         foreach (var line in Connections.Keys)
//         {
//             foreach (var station in Connections[line].Keys)
//             {
//                 foreach (var connection in Connections[line][station])
//                 {
//                     if (!connection.Open)
//                     {
//                         closedTracksMessage = closedTracksMessage + $"Connection from {station} to {connection.DestinationStation.Name} on {line}\n";
//                     }
//                     if (connection.Delay > 0)
//                     {
//                         delaysMessage = delaysMessage + $"Connection from {station} to {connection.DestinationStation.Name} on {line} with Delay {connection.Delay} mins\n";
//                     }
//                 }
//             }
//         }
//         switch (input)
//         {
//             case "closed":
//             return closedTracksMessage;
//
//             case "delays":
//             return delaysMessage;
//         }
//         return "";
//     }
//
//     public static void HandleUserInput(string response, List<string> options)
//     {
//         switch (response)
//         {
//             case "calculate shortest path":
//                 var shortest = ShortestPath(options[0], options[1]);
//                 PrintShortestPath(shortest);
//                 break;
//             
//             case "Add Track Section Delay":
//                 Connections[options[0]][options[1]][int.Parse(options[2])].Delay = int.Parse(options[3]);
//                 break;
//             
//             case "Remove Track Section Delay":
//                 Connections[options[0]][options[1]][int.Parse(options[2])].Delay = 0;
//                 break;
//             
//             case "Open Track Section":
//                 Connections[options[0]][options[1]][int.Parse(options[2])].Open = true;
//                 break;
//             
//             case "Close Track Section":
//                 Connections[options[0]][options[1]][int.Parse(options[2])].Open = false;
//                 break;
//             
//             case "Print Station Information":
//                 Station station = Stations[options[0]];
//                 Console.WriteLine("-------------------------------------");
//                 Console.WriteLine($"Station Details for: {station.Name}");
//                 Console.WriteLine("Connections:");
//                 foreach (var connection in station.Connections)
//                 {
//                     Console.WriteLine($"  - To: {connection.DestinationStation.Name}, Travel time: {connection.TravelTime}, Delay: {connection.Delay}, Open: {connection.Open.ToString()}, Line: {connection.Line}, Direction: {connection.Direction}");
//                 }
//                 break;
//         }
//     }
//
//     private static void AddStation(string name)
//     {
//         Stations[name] = new Station(name);
//     }
//
//     private static void AddConnection(string station1, string station2, double travelTime, string line, string direction)
//     {
//         //TODO Refactor to try get value
//         if (!Stations.ContainsKey(station1) || !Stations.ContainsKey(station2))
//         {
//             throw new ArgumentException("One or both of the stations do not exist.");
//         }
//         
//         Stations[station1].AddConnection(Stations[station2], travelTime, line, direction);
//         
//         //Adding all connections for station1 in at the given line to the Connections list
//         if (Connections[line].ContainsKey(station1))
//         {
//             // If the inner key exists, add the item to the list associated with that inner key
//             Connections[line][station1].Add(Stations[station1].Connections.Last());
//         }
//         else
//         {
//             // If the inner key does not exist, create a new list and add the item to it
//             var newList = new List<Connection> { Stations[station1].Connections.Last() };
//                 
//             // Add the inner key along with the list to the inner dictionary
//             Connections[line].Add(station1, newList);
//         }
//     }
//     private static void LoadDataFromCSV(string filePath) 
//     { 
//         using (var reader = new StreamReader(filePath)) 
//         {
//             // Skip the header line
//             reader.ReadLine();
//
//             while (!reader.EndOfStream)
//             {
//                 var line = reader.ReadLine();
//                 var values = line.Split(',');
//
//                 string lineName = values[0];
//                 string direction = values[1];
//                 string stationA = values[2];
//                 string stationB = values[3];
//                 double distanceKM, unImpededTime;
//                 if (!double.TryParse(values[4], out distanceKM) || !double.TryParse(values[5], out unImpededTime))
//                 {
//                     //Console.WriteLine("Error parsing distance or time values in line: " + line);
//                     continue; // Skip this line and move to the next one
//                 }
//
//                 string trainLine = lineName;
//
//                 // Add stations and connections
//                 if (!Stations.ContainsKey(stationA))
//                 {
//                     AddStation(stationA);
//                 }
//                 if (!Stations.ContainsKey(stationB))
//                 {
//                     AddStation(stationB);
//                 }
//                 
//                 if (!Connections.ContainsKey(trainLine))
//                 {
//                     Connections[trainLine] = new CustomDictionary<string, List<Connection>>();
//                 }
//
//                 // Using un-impeded running time as weight
//                 double travelTime = unImpededTime;
//
//                 AddConnection(stationA, stationB, travelTime, trainLine, direction);
//                 
//             }
//             
//         }
//     }
//
//     private static void ResetStationData()
//     {
//         foreach (var station in Stations.Values)
//         {
//             station.TimeFromStart = double.PositiveInfinity;
//             station.Visited = false;
//             station.Previous = null;
//             station.currentLine = "";
//         }
//     }
//
//     private static (List<(Station, string)>, int) ShortestPath(string startName, string endName)
//     {
//         int changes = 0;        
//         //Reset all stations:
//         ResetStationData();
//         
//         //Assign start station
//         Station startStation = Stations[startName];
//         startStation.TimeFromStart = 0;
//
//         var comparer = new StationComparer();
//         //Comparer object passed into sorted set, and ordering the stations in this sortedSet by time from start
//         var unexploredStations = new SortedSet<Station>(comparer);
//         unexploredStations.Add(startStation);
//         
//         //Finding the shortest path from start to end
//
//         //While there are stations to be explored
//         while (unexploredStations.Count > 0)
//         {
//             //Current station is the one at the front of the queue (i.e unexploredStations.min)
//             var currentStation = unexploredStations.Min;
//
//             foreach (var connection in currentStation.Connections)
//             {
//                 if (connection.Open)
//                 {
//                     int change = 0;
//                     var neighborStation = connection.DestinationStation;
//                     if (currentStation.currentLine != connection.Line && currentStation.currentLine != "")
//                     {    
//                         change = 2;
//                     }
//                     
//                     var tentativeTravelTime = currentStation.TimeFromStart + connection.Delay + connection.TravelTime + change;
//
//                     
//                     //Compare current neighbouring stations time from start with the current stations time from start, plus connection weight
//                     //to neighbouring station
//                     if (tentativeTravelTime < neighborStation.TimeFromStart )
//                     {
//                         neighborStation.TimeFromStart = tentativeTravelTime;
//                         neighborStation.Previous = currentStation;
//                         neighborStation.currentLine = connection.Line;
//
//                         if (!unexploredStations.Contains(neighborStation))
//                         {
//                             unexploredStations.Add(neighborStation);
//                         }
//                     }
//                 }
//             }
//             //After current station has been explored, remove it from the front of the queue    
//             unexploredStations.Remove(currentStation);
//             currentStation.Visited = true;
//         }
//         
//         //A list of tuples, current station (Station) and a string holding, previous station and line taken (String)
//         var shortestPath = new List<(Station, string)>();
//         Station next = null;
//         var current = Stations[endName];
//         var currentLine = "";
//         Connection previousConnection = null;
//         Connection nextConnection = null;
//         
//         //Build path from end to start based on the .Previous attributes of each station
//         while (current != null)
//         {
//             if (current.Previous != null)
//             {
//                 current.Next = next;
//                 //The previous connection is found by finding the previous station, whos connection = our current station (and is open)
//                 previousConnection = current.Previous.Connections.FirstOrDefault(c => c.DestinationStation == current && c.Open != false);
//                 if (current != Stations[endName])
//                 {
//                     nextConnection = current.Connections.FirstOrDefault(c => c.DestinationStation.Name == current.Next.Name & c.Open != false);   
//                 }
//                 if (previousConnection != null)
//                 {
//                     //If the line or direction changes, update the current line, and add line change message
//                     if ($"{previousConnection.Line} {previousConnection.Direction}" != currentLine)
//                         {
//                             if (currentLine != "")
//                             {
//                                 var lineChangeMessage = "";
//                                 if (next != null)
//                                 {
//                                     changes += 2;
//                                     //shortestPath.Last().Item1.TimeFromStart += 2;
//                                     lineChangeMessage = $"Change: {string.Join(",", previousConnection.Line)} {previousConnection.Direction} to {nextConnection.Line} ({nextConnection.Direction}) 2.00 mins";   
//                                 }
//                                 shortestPath.Insert(0, (current, lineChangeMessage));
//                             }
//                             
//                             currentLine = $"{previousConnection.Line} {previousConnection.Direction}";                      
//                         }      
//                     }
//             }
//
//             next = current;
//             shortestPath.Insert(0, (current, currentLine));
//             current = current.Previous;
//         }
//         
//         return (shortestPath, changes);
//     }
//
//         private static void PrintShortestPath((List<(Station, string)>, int) shortestPath)
//         {
//         Console.Clear();
//         
//         Console.WriteLine($"Route: {shortestPath.Item1.First().Item1.Name} to {shortestPath.Item1.Last().Item1.Name}:");
//         
//         Station prev = null;
//         foreach (var stationTuple in shortestPath.Item1)
//         {
//             var station = stationTuple.Item1;
//             var line = stationTuple.Item2;
//
//             if (station != prev)
//             {
//                 if (station.Previous != null)
//                 {
//                     Console.WriteLine("(" + (shortestPath.Item1.IndexOf(stationTuple)+1) + ") " + station.Name + " (Line: " + line +")" + " " + (station.TimeFromStart - station.Previous.TimeFromStart));   
//                 }
//                 else
//                 {
//                     Console.WriteLine("(" + (shortestPath.Item1.IndexOf(stationTuple)+1) + ") " + station.Name + " (Line: " + line +")");
//                 }
//                 prev = station;
//             }
//             else
//             {
//                 Console.WriteLine(line);    
//             }
//         }
//         
//         Console.WriteLine($"Total Journey Time: {shortestPath.Item1.Last().Item1.TimeFromStart} minutes");
//     }
// }