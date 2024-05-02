namespace TFLTripPlannerHandCoded;

public class CustomList<T>
{
    private T[] items;
    private int capacity; 
    public int Count { get; private set; }
        
    public CustomList()
    {
        capacity = 8;
        items = new T[capacity];
        Count = 0;
    }
    
    public void Add(T item)
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
        T[] newItems = new T[capacity];
        for (int i = 0; i < Count; i++)
        {
            newItems[i] = items[i];
        }
        items = newItems;
    }

    public T this[int index]
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
    
    //TODO Need Insert function
    //TODO Need First function
    //TODO Need Last function
    //TODO Need IndexOf function
    //Note: See London Underground for where and how these functions are used
    
    //TODO Need Clear function for functionality in ConsoleView
}

