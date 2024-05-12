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

    public void Insert(int index, T item)
    {
        if (index < 0 || index > Count)
            throw new ArgumentOutOfRangeException("Index out of range");
        if (Count >= capacity)
            IncreaseCapacity();

        for (int i = Count; i > index; i--)
        {
            items[i] = items[i - 1];
        }

        items[index] = item;
        Count++;
    }

    public T First()
    {
        if (Count == 0)
            throw new InvalidOperationException("The list is empty.");
        return items[0];
    }

    public T FirstOrDefault(Func<T, bool> condition)
    {
        for (int i = 0; i < Count; i++)
        {
            if (condition(items[i]))
                return items[i];
        }

        return default(T);
    }

    public T Last()
    {
        if (Count == 0)
            throw new InvalidOperationException("The list is empty.");
        return items[Count - 1];
    }

    public int IndexOf(T item)
    {
        for (int i = 0; i < Count; i++)
        {
            if (items[i] == null && item == null)
                return i;
            if (items[i] != null && items[i].Equals(item))
                return i;
        }

        return -1;
    }

    public void Clear()
    {
        for (int i = 0; i < Count; i++)
        {
            items[i] = default(T);
        }

        Count = 0;
    }
}