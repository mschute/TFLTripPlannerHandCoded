namespace TFLTripPlannerHandCoded;

public class CustomList<IEnumerable>
{
    private IEnumerable[] items;
    private int capacity; 
    public int Count { get; private set; }
        
    public CustomList()
    {
        capacity = 8;
        items = new IEnumerable[capacity];
        Count = 0;
    }
    
    public void Add(IEnumerable item)
    {
        if (Count >= capacity)
        {
            IncreaseCapacity();
        }
        items[Count++] = item;
    }

    private void IncreaseCapacity()
    {
        capacity *= 2;
        IEnumerable[] newItems = new IEnumerable[capacity];
        for (int i = 0; i < Count; i++)
        {
            newItems[i] = items[i];
        }
        items = newItems;
    }

    public IEnumerable this[int index]
    {
        get
        {
            if (index < 0 || index >= Count)
                throw new ArgumentOutOfRangeException("Index out of range");
            return items[index];
        }
        set
        {
            if (index < 0 || index >= Count)
                throw new ArgumentOutOfRangeException("Index out of range");
            items[index] = value;
        }
    }
}

