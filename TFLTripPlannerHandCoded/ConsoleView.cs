namespace TFLTripPlannerHandCoded;

public class ConsoleView
{
    public MenuItem[] mainMenuItems = new MenuItem[3];
    CustomList<string> temp = new CustomList<string>();

    public ConsoleView()
    {
        Console.CursorVisible = false;
        InitializeMenu();
        NavigateMenu(mainMenuItems);
    }

    private void InitializeMenu()
    {
        mainMenuItems[0] = new MenuItem("Customer", new MenuItem[]
        {
            new MenuItem("calculate shortest path", null),
            new MenuItem("Back", null)
        });

        mainMenuItems[1] = new MenuItem("Engineer", new MenuItem[]
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

        mainMenuItems[2] = new MenuItem("Exit", null);

        foreach (MenuItem m in mainMenuItems[0].Submenu)
        {
            m.SetParentMenu(mainMenuItems[0]);
        }

        foreach (MenuItem m in mainMenuItems[1].Submenu)
        {
            m.SetParentMenu(mainMenuItems[1]);
        }

        mainMenuItems[0].Submenu[0].SetSubMenu(generateMenuList(mainMenuItems[0].Submenu[0], "allStations"));

        //TODO This menu is for removing delays but its showing all stations not just the ones with delays set.
        mainMenuItems[1].Submenu[0].SetSubMenu(generateMenuList(mainMenuItems[1].Submenu[0], "allLines"));

        mainMenuItems[1].Submenu[1].SetSubMenu(generateMenuList(mainMenuItems[1].Submenu[1], "allLines"));

        //TODO This menu is for removing closures on tracks but its showing all stations not just the ones closed.
        mainMenuItems[1].Submenu[2].SetSubMenu(generateMenuList(mainMenuItems[1].Submenu[2], "allLines"));

        mainMenuItems[1].Submenu[3].SetSubMenu(generateMenuList(mainMenuItems[1].Submenu[3], "allLines"));
        mainMenuItems[1].Submenu[6].SetSubMenu(generateMenuList(mainMenuItems[1].Submenu[6], "allStations"));
    }

    //TODO Refactor this code.
    public MenuItem[] generateMenuList(MenuItem parent, string menu)
    {
        MenuItem[] stationsSub;

        if (menu == "allStations")
        {
            stationsSub = new MenuItem[LondonUnderground.Stations.Keys.Count + 1];
            int index = 0;
            for (int i = 0; i < LondonUnderground.Stations.Keys.Count; i++)
            {
                var newStat = (new MenuItem(LondonUnderground.Stations.Keys[i], null));
                newStat.SetParentMenu(parent);
                stationsSub[index] = newStat;
                index++;
            }

            ;
            stationsSub[LondonUnderground.Stations.Keys.Count] = new MenuItem("Back", null);
        }
        else if (menu == "allLines")
        {
            stationsSub = new MenuItem[LondonUnderground.Connections.Keys.Count + 1];
            int index = 0;
            //TODO Change to for loop
            for (int i = 0; i < LondonUnderground.Connections.Keys.Count; i++)
            {
                var newLine = (new MenuItem(LondonUnderground.Connections.Keys[i], null));
                newLine.SetParentMenu(parent);
                stationsSub[index] = newLine;
                stationsSub[index].SetSubMenu(generateMenuList(newLine, "stationsInLine"));
                index++;
            }

            ;
            stationsSub[LondonUnderground.Connections.Keys.Count] = new MenuItem("Back", null);
        }
        else if (menu == "stationsInLine")
        {
            stationsSub = new MenuItem[LondonUnderground.Connections[parent.Label].Keys.Count + 1];

            for (int i = 0; i < LondonUnderground.Connections[parent.Label].Keys.Count; i++)
            {
                var newStat = (new MenuItem(LondonUnderground.Connections[parent.Label].Keys[i], null));
                newStat.SetParentMenu(parent);
                stationsSub[i] = newStat;
                stationsSub[i].SetSubMenu(generateMenuList(newStat, "connectionsForStation"));
            }

            ;
            stationsSub[LondonUnderground.Connections[parent.Label].Keys.Count] = new MenuItem("Back", null);
        }
        else if (menu == "connectionsForStation")
        {
            stationsSub = new MenuItem[LondonUnderground.Connections[parent.Parent.Label][parent.Label].Count + 1];

            for (int i = 0; i < LondonUnderground.Connections[parent.Parent.Label][parent.Label].Count; i++)
            {
                var connection = LondonUnderground.Connections[parent.Parent.Label][parent.Label][i];
                var newStat =
                    (new MenuItem(
                        $"{connection.DestinationStation.Name} {connection.Direction} {connection.TravelTime}", null));
                newStat.SetParentMenu(parent);
                stationsSub[i] = newStat;
            }

            ;
            stationsSub[LondonUnderground.Connections[parent.Parent.Label][parent.Label].Count] =
                new MenuItem("Back", null);
        }
        else
        {
            stationsSub = new MenuItem[LondonUnderground.Connections[parent.Label].Keys.Count + 1];
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
                        if (LondonUnderground.Stations.ContainsKey(menuItems[selectedItemIndex].Label))
                        {
                            if (menuItems[selectedItemIndex].Parent.Parent.Label == "Customer")
                            {
                                if (temp.Count < 2)
                                {
                                    temp.Add(menuItems[selectedItemIndex].Label);
                                }
                                else
                                {
                                    temp.Clear();
                                    temp.Add(menuItems[selectedItemIndex].Label);
                                }

                                if (temp.Count == 2)
                                {
                                    LondonUnderground.HandleUserInput(menuItems[selectedItemIndex].Parent.Label, temp);
                                }
                            }
                            else
                            {
                                temp.Clear();
                                temp.Add(menuItems[selectedItemIndex].Label);
                                LondonUnderground.HandleUserInput(menuItems[selectedItemIndex].Parent.Label, temp);
                            }
                        }
                        else if (menuItems[selectedItemIndex].Parent.Parent != null)
                        {
                            if (LondonUnderground.Connections.ContainsKey(menuItems[selectedItemIndex].Parent.Parent
                                    .Label))
                            {
                                temp.Clear();
                                temp.Add(menuItems[selectedItemIndex].Parent.Parent.Label);
                                temp.Add(menuItems[selectedItemIndex].Parent.Label);
                                temp.Add(selectedItemIndex.ToString());

                                if (menuItems[selectedItemIndex].Parent.Parent.Parent.Label ==
                                    "Add Track Section Delay")
                                {
                                    //TODO Need to validate this a a valid int.
                                    Console.WriteLine("Enter delay time in (mins)");
                                    string number = Console.ReadLine();
                                    temp.Add(number);
                                }

                                LondonUnderground.HandleUserInput(
                                    menuItems[selectedItemIndex].Parent.Parent.Parent.Label, temp);
                            }
                        }
                        else
                        {
                            LondonUnderground.HandleUserInput(menuItems[selectedItemIndex].Label);
                        }

                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey(true);
                    }

                    break;
            }
        }
    }
}