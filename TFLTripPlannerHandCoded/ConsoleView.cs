namespace TFLTripPlannerHandCoded;

public class ConsoleView
{
    private const string AddTrackSectionDelay = "Add Track Section Delay";
    private const string AllLines = "allLines";
    private const string AllStations = "allStations";
    private const string Back = "Back";
    private const string CalculateShortestPath = "Calculate Shortest Path";
    private const string CloseTrackSection = "Close Track Section";
    private const string ConnectionsForStation = "connectionsForStation";
    private const string Customer = "Customer";
    private const string Engineer = "Engineer";
    private const string Exit = "Exit";
    private const string OpenTrackSection = "Open Track Section";
    private const string PrintClosedTrackSections = "Print Closed Track Sections";
    private const string PrintStationInformation = "Print Station Information";
    private const string PrintTrackSectionDelays = "Print Track Section Delays";
    private const string RemoveTrackSectionDelay = "Remove Track Section Delay";
    private const string StationsInLine = "stationsInLine";

    public delegate void OnShortestPathHandler(string start, string end);

    public delegate void OnPrintStationHandler(string name);

    public delegate void OnPrintStationClosuresHandler();

    public delegate void OnPrintStationDelaysHandler();

    public delegate void OnTrackSectionOpenHandler(string line, string station, int connection, bool open);

    public delegate void OnTrackSectionDelayHandler(string line, string station, int connection, int delay);

    public OnShortestPathHandler OnShortestPath;
    public OnPrintStationHandler OnPrintStation;
    public OnTrackSectionOpenHandler OnTrackSectionOpen;
    public OnTrackSectionDelayHandler OnTrackSectionDelay;
    public OnPrintStationClosuresHandler OnPrintStationClosures;
    public OnPrintStationDelaysHandler OnPrintStationDelays;

    private readonly IStationMap _stationMap;
    private MenuItem[] MainMenuItems = new MenuItem[3];
    private readonly CustomList<string> _temp = new();

    public ConsoleView(IStationMap stationMap)
    {
        _stationMap = stationMap;

        Console.CursorVisible = false;
        InitializeMenu();
    }

    public void EnterMainMenu()
    {
        NavigateMenu(MainMenuItems);
    }

    private void InitializeMenu()
    {
        MainMenuItems[0] = new MenuItem(Customer, new MenuItem[]
        {
            new MenuItem(CalculateShortestPath, null),
            new MenuItem(Back, null)
        });

        MainMenuItems[1] = new MenuItem(Engineer, new MenuItem[]
        {
            new MenuItem(AddTrackSectionDelay, null),
            new MenuItem(RemoveTrackSectionDelay, null),
            new MenuItem(OpenTrackSection, null),
            new MenuItem(CloseTrackSection, null),
            new MenuItem(PrintClosedTrackSections, null),
            new MenuItem(PrintTrackSectionDelays, null),
            new MenuItem(PrintStationInformation, null),
            new MenuItem(Back, null)
        });

        MainMenuItems[2] = new MenuItem(Exit, null);

        foreach (MenuItem m in MainMenuItems[0].Submenu)
        {
            m.SetParentMenu(MainMenuItems[0]);
        }

        foreach (MenuItem m in MainMenuItems[1].Submenu)
        {
            m.SetParentMenu(MainMenuItems[1]);
        }

        MainMenuItems[0].Submenu[0].SetSubMenu(GenerateMenuList(MainMenuItems[0].Submenu[0], AllStations));

        MainMenuItems[1].Submenu[0].SetSubMenu(GenerateMenuList(MainMenuItems[1].Submenu[0], AllLines));

        MainMenuItems[1].Submenu[1].SetSubMenu(GenerateMenuList(MainMenuItems[1].Submenu[1], AllLines));

        MainMenuItems[1].Submenu[2].SetSubMenu(GenerateMenuList(MainMenuItems[1].Submenu[2], AllLines));

        MainMenuItems[1].Submenu[3].SetSubMenu(GenerateMenuList(MainMenuItems[1].Submenu[3], AllLines));

        MainMenuItems[1].Submenu[6].SetSubMenu(GenerateMenuList(MainMenuItems[1].Submenu[6], AllStations));
    }

    private MenuItem[] GenerateMenuList(MenuItem parent, string menu)
    {
        MenuItem[] stationsSub;

        if (menu == AllStations)
        {
            var stationKeys = _stationMap.Stations.Keys;

            stationsSub = new MenuItem[stationKeys.Count + 1];
            for (var i = 0; i < stationKeys.Count; i++)
            {
                var newStat = (new MenuItem(stationKeys[i], null));
                newStat.SetParentMenu(parent);
                stationsSub[i] = newStat;
            }

            stationsSub[stationKeys.Count] = new MenuItem(Back, null);
        }
        else if (menu == AllLines)
        {
            var connectionKeys = _stationMap.Connections.Keys;

            stationsSub = new MenuItem[connectionKeys.Count + 1];

            for (var i = 0; i < connectionKeys.Count; i++)
            {
                var newLine = (new MenuItem(connectionKeys[i], null));
                newLine.SetParentMenu(parent);
                stationsSub[i] = newLine;
                stationsSub[i].SetSubMenu(GenerateMenuList(newLine, StationsInLine));
            }

            stationsSub[connectionKeys.Count] = new MenuItem(Back, null);
        }
        else if (menu == StationsInLine)
        {
            var connectionParentKeys = _stationMap.Connections[parent.Label].Keys;

            stationsSub = new MenuItem[connectionParentKeys.Count + 1];

            for (var i = 0; i < connectionParentKeys.Count; i++)
            {
                var newStat = (new MenuItem(connectionParentKeys[i], null));
                newStat.SetParentMenu(parent);
                stationsSub[i] = newStat;
                stationsSub[i].SetSubMenu(GenerateMenuList(newStat, ConnectionsForStation));
            }

            stationsSub[connectionParentKeys.Count] = new MenuItem(Back, null);
        }
        else if (menu == ConnectionsForStation)
        {
            stationsSub = new MenuItem[_stationMap.Connections[parent.Parent.Label][parent.Label].Count + 1];

            for (var i = 0; i < _stationMap.Connections[parent.Parent.Label][parent.Label].Count; i++)
            {
                var connection = _stationMap.Connections[parent.Parent.Label][parent.Label][i];
                var newStat =
                    (new MenuItem(
                        $"{connection.DestinationStation.Name} {connection.Direction} {connection.TravelTime}", null));
                newStat.SetParentMenu(parent);
                stationsSub[i] = newStat;
            }

            stationsSub[_stationMap.Connections[parent.Parent.Label][parent.Label].Count] = new MenuItem(Back, null);
        }
        else
        {
            stationsSub = new MenuItem[_stationMap.Connections[parent.Label].Keys.Count + 1];
        }

        return stationsSub;
    }

    private void NavigateMenu(MenuItem[] menuItems)
    {
        var startIndex = 0;
        var endIndex = Console.WindowHeight - 1;

        var selectedItemIndex = 0;

        while (true)
        {
            Console.Clear();
            for (var i = startIndex; i <= endIndex && i < menuItems.Length; i++)
            {
                if (i == selectedItemIndex)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Gray;
                }

                Console.WriteLine(menuItems[i].Label);
            }

            Console.ResetColor();

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    if (selectedItemIndex > 0)
                    {
                        selectedItemIndex--;
                        if (selectedItemIndex < startIndex)
                        {
                            startIndex--;
                            endIndex--;
                        }
                    }

                    break;
                case ConsoleKey.DownArrow:
                    if (selectedItemIndex < menuItems.Length - 1)
                    {
                        selectedItemIndex++;
                        if (selectedItemIndex > endIndex)
                        {
                            startIndex++;
                            endIndex++;
                        }
                    }

                    break;

                case ConsoleKey.LeftArrow:
                    return;

                case ConsoleKey.Enter:
                case ConsoleKey.RightArrow:
                    if (menuItems[selectedItemIndex].Submenu != null)
                    {
                        // Navigate to submenu
                        NavigateMenu(menuItems[selectedItemIndex].Submenu);
                    }
                    else if (menuItems[selectedItemIndex].Label == Back)
                    {
                        return;
                    }
                    else if (menuItems[selectedItemIndex].Label == Exit)
                    {
                        Console.WriteLine("Process Terminated, Press any key to close");
                        Environment.Exit(0);
                    }
                    else
                    {
                        // Perform action based on selected menu item
                        if (_stationMap.Stations.ContainsKey(menuItems[selectedItemIndex].Label))
                        {
                            if (menuItems[selectedItemIndex].Parent.Parent.Label == Customer)
                            {
                                if (_temp.Count < 2)
                                {
                                    _temp.Add(menuItems[selectedItemIndex].Label);
                                }
                                else
                                {
                                    _temp.Clear();
                                    _temp.Add(menuItems[selectedItemIndex].Label);
                                }

                                if (_temp.Count == 2)
                                {
                                    HandleUserInput(menuItems[selectedItemIndex].Parent.Label, _temp);
                                }
                            }
                            else
                            {
                                _temp.Clear();
                                _temp.Add(menuItems[selectedItemIndex].Label);
                                HandleUserInput(menuItems[selectedItemIndex].Parent.Label, _temp);
                            }
                        }
                        else if (menuItems[selectedItemIndex].Parent.Parent != null)
                        {
                            if (_stationMap.Connections.ContainsKey(menuItems[selectedItemIndex].Parent.Parent
                                    .Label))
                            {
                                _temp.Clear();
                                _temp.Add(menuItems[selectedItemIndex].Parent.Parent.Label);
                                _temp.Add(menuItems[selectedItemIndex].Parent.Label);
                                _temp.Add(selectedItemIndex.ToString());

                                if (menuItems[selectedItemIndex].Parent.Parent.Parent.Label ==
                                    AddTrackSectionDelay)
                                {
                                    Console.WriteLine("Enter delay time in (mins)");
                                    var number = Console.ReadLine();
                                    _temp.Add(number);
                                }

                                HandleUserInput(menuItems[selectedItemIndex].Parent.Parent.Parent.Label, _temp);
                            }
                        }
                        else
                        {
                            HandleUserInput(menuItems[selectedItemIndex].Label);
                        }

                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey(true);
                    }

                    break;
            }
        }
    }

    private void HandleUserInput(string response, CustomList<string> options)
    {
        switch (response)
        {
            case CalculateShortestPath:
                var start = options[0];
                var end = options[1];
                OnShortestPath(start, end);
                break;

            case AddTrackSectionDelay:
            {
                var line = options[0];
                var station = options[1];
                var connection = int.Parse(options[2]);
                var delay = int.Parse(options[3]);
                OnTrackSectionDelay(line, station, connection, delay);
                break;
            }

            case RemoveTrackSectionDelay:
            {
                var line = options[0];
                var station = options[1];
                var connection = int.Parse(options[2]);
                const int delay = 0;
                OnTrackSectionDelay(line, station, connection, delay);
                break;
            }

            case OpenTrackSection:
            {
                var line = options[0];
                var station = options[1];
                var connection = int.Parse(options[2]);
                OnTrackSectionOpen(line, station, connection, true);
                break;
            }

            case CloseTrackSection:
            {
                var line = options[0];
                var station = options[1];
                var connection = int.Parse(options[2]);
                OnTrackSectionOpen(line, station, connection, false);
                break;
            }

            case PrintStationInformation:
            {
                var station = options[0];
                OnPrintStation(station);
                break;
            }
        }
    }

    private void HandleUserInput(string response)
    {
        switch (response)
        {
            case PrintClosedTrackSections:
                OnPrintStationClosures();
                break;

            case PrintTrackSectionDelays:
                OnPrintStationDelays();
                break;
        }
    }
}