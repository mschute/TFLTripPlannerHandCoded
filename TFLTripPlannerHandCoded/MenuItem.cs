namespace TFLTripPlannerHandCoded;

public class MenuItem
{
    public string Label { get; }
    public MenuItem Parent { get; set; }
    public MenuItem[] Submenu { get; set; }

    public MenuItem(string label, MenuItem[] submenu)
    {
        Label = label;
        Parent = null;
        Submenu = submenu;
    }

    public MenuItem(string label, MenuItem parent, MenuItem[] submenu)
    {
        Label = label;
        Parent = parent;
        Submenu = submenu;
    }

    public void SetParentMenu(MenuItem parent)
    {
        Parent = parent;
    }

    public void SetSubMenu(MenuItem[] sub)
    {
        Submenu = sub;
    }
}