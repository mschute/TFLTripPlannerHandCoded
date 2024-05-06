namespace TFLTripPlannerHandCoded;

public class ConsoleView
{
    private readonly LondonUnderground _londonUnderground;
    
    public MenuItem[] MainMenuItems = new MenuItem[3];
    private CustomList<string> _temp = new CustomList<string>();

    public ConsoleView(LondonUnderground londonUnderground)
    {
        _londonUnderground = londonUnderground;
        
        Console.CursorVisible = false;
        InitializeMenu();
        NavigateMenu(MainMenuItems);
    }

    private void InitializeMenu()
    {
        MainMenuItems[0] = new MenuItem("Customer", new MenuItem[]
        {
            new MenuItem("Calculate Shortest Path", null),
            new MenuItem("Back", null)
        });

        MainMenuItems[1] = new MenuItem("Engineer", new MenuItem[]
        {
            new MenuItem("Add Track Section Delay", null),
            new MenuItem("Remove Track Section Delay", null),
            new MenuItem("Open Track Section", null),
            new MenuItem("Close Track Section", null),
            new MenuItem("Print Closed Track Sections", null),
            new MenuItem("Print Track Section Delays", null),
            new MenuItem("Print Station Information", null),
            new MenuItem("Back", null)
        });

        MainMenuItems[2] = new MenuItem("Exit", null);

        foreach (MenuItem m in MainMenuItems[0].Submenu)
        {
            m.SetParentMenu(MainMenuItems[0]);
        }

        foreach (MenuItem m in MainMenuItems[1].Submenu)
        {
            m.SetParentMenu(MainMenuItems[1]);
        }

        MainMenuItems[0].Submenu[0].SetSubMenu(GenerateMenuList(MainMenuItems[0].Submenu[0], "allStations"));

        //TODO This menu is for removing delays but its showing all stations not just the ones with delays set.
        MainMenuItems[1].Submenu[0].SetSubMenu(GenerateMenuList(MainMenuItems[1].Submenu[0], "allLines"));

        MainMenuItems[1].Submenu[1].SetSubMenu(GenerateMenuList(MainMenuItems[1].Submenu[1], "allLines"));

        //TODO This menu is for removing closures on tracks but its showing all stations not just the ones closed.
        MainMenuItems[1].Submenu[2].SetSubMenu(GenerateMenuList(MainMenuItems[1].Submenu[2], "allLines"));

        MainMenuItems[1].Submenu[3].SetSubMenu(GenerateMenuList(MainMenuItems[1].Submenu[3], "allLines"));
        MainMenuItems[1].Submenu[6].SetSubMenu(GenerateMenuList(MainMenuItems[1].Submenu[6], "allStations"));
    }

    //TODO Refactor this code.
    public MenuItem[] GenerateMenuList(MenuItem parent, string menu)
    {
        MenuItem[] stationsSub;

        if (menu == "allStations")
        {
            var stationKeys = _londonUnderground.Stations.Keys;
            
            stationsSub = new MenuItem[stationKeys.Count + 1];
            for (int i = 0; i < stationKeys.Count; i++)
            {
                var newStat = (new MenuItem(stationKeys[i], null));
                newStat.SetParentMenu(parent);
                stationsSub[i] = newStat;
            }
            
            stationsSub[stationKeys.Count] = new MenuItem("Back", null);
        }
        else if (menu == "allLines")
        {
            var connectionKeys = _londonUnderground.Connections.Keys;
            
            stationsSub = new MenuItem[connectionKeys.Count + 1];
            
            for (int i = 0; i < connectionKeys.Count; i++)
            {
                var newLine = (new MenuItem(connectionKeys[i], null));
                newLine.SetParentMenu(parent);
                stationsSub[i] = newLine;
                stationsSub[i].SetSubMenu(GenerateMenuList(newLine, "stationsInLine"));
            }
            
            stationsSub[connectionKeys.Count] = new MenuItem("Back", null);
        }
        else if (menu == "stationsInLine")
        {
            var connectionParentKeys = _londonUnderground.Connections[parent.Label].Keys;
            
            stationsSub = new MenuItem[connectionParentKeys.Count + 1];

            for (int i = 0; i < connectionParentKeys.Count; i++)
            {
                var newStat = (new MenuItem(connectionParentKeys[i], null));
                newStat.SetParentMenu(parent);
                stationsSub[i] = newStat;
                stationsSub[i].SetSubMenu(GenerateMenuList(newStat, "connectionsForStation"));
            }
            
            stationsSub[connectionParentKeys.Count] = new MenuItem("Back", null);
        }
        else if (menu == "connectionsForStation")
        {
            stationsSub = new MenuItem[_londonUnderground.Connections[parent.Parent.Label][parent.Label].Count + 1];

            for (int i = 0; i < _londonUnderground.Connections[parent.Parent.Label][parent.Label].Count; i++)
            {
                var connection = _londonUnderground.Connections[parent.Parent.Label][parent.Label][i];
                var newStat =
                    (new MenuItem(
                        $"{connection.DestinationStation.Name} {connection.Direction} {connection.TravelTime}", null));
                newStat.SetParentMenu(parent);
                stationsSub[i] = newStat;
            }
            
            stationsSub[_londonUnderground.Connections[parent.Parent.Label][parent.Label].Count] = new MenuItem("Back", null);
        }
        else
        {
            stationsSub = new MenuItem[_londonUnderground.Connections[parent.Label].Keys.Count + 1];
        }

        return stationsSub;
    }

    private void NavigateMenu(MenuItem[] menuItems)
    {
        int startIndex = 0; // Index of the first visible item
        int endIndex = Console.WindowHeight - 1; // Index of the last visible item

        int selectedItemIndex = 0;

        while (true)
        {
            Console.Clear();
            for (int i = startIndex; i <= endIndex && i < menuItems.Length; i++)
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
                    else if (menuItems[selectedItemIndex].Label == "Back")
                    {
                        return;
                    }
                    else if (menuItems[selectedItemIndex].Label == "Exit")
                    {
                        // Exit the application
                        Console.WriteLine("Process Terminated, Press any key to close");
                        Environment.Exit(0);
                    }
                    else
                    {
                        // Perform action based on selected menu item
                        if (_londonUnderground.Stations.ContainsKey(menuItems[selectedItemIndex].Label))
                        {
                            if (menuItems[selectedItemIndex].Parent.Parent.Label == "Customer")
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
                                    _londonUnderground.HandleUserInput(menuItems[selectedItemIndex].Parent.Label, _temp);
                                }
                            }
                            else
                            {
                                _temp.Clear();
                                _temp.Add(menuItems[selectedItemIndex].Label);
                                _londonUnderground.HandleUserInput(menuItems[selectedItemIndex].Parent.Label, _temp);
                            }
                        }
                        else if (menuItems[selectedItemIndex].Parent.Parent != null)
                        {
                            if (_londonUnderground.Connections.ContainsKey(menuItems[selectedItemIndex].Parent.Parent
                                    .Label))
                            {
                                _temp.Clear();
                                _temp.Add(menuItems[selectedItemIndex].Parent.Parent.Label);
                                _temp.Add(menuItems[selectedItemIndex].Parent.Label);
                                _temp.Add(selectedItemIndex.ToString());

                                if (menuItems[selectedItemIndex].Parent.Parent.Parent.Label ==
                                    "Add Track Section Delay")
                                {
                                    //TODO Need to validate this a a valid int.
                                    Console.WriteLine("Enter delay time in (mins)");
                                    string number = Console.ReadLine();
                                    _temp.Add(number);
                                }

                                _londonUnderground.HandleUserInput(
                                    menuItems[selectedItemIndex].Parent.Parent.Parent.Label, _temp);
                            }
                        }
                        else
                        {
                            _londonUnderground.HandleUserInput(menuItems[selectedItemIndex].Label);
                        }

                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey(true);
                    }

                    break;
            }
        }
    }
}