namespace TFLTripPlannerHandCoded;

public class CustomDictionary<TKey, TValue>
    where TKey : IComparable<TKey>
{
    private readonly BinarySearchTree<TKey, TValue> _tree = new();

    public TValue this[TKey key]
    {
        get
        {
            try
            {
                var node = _tree.GetNode(key);

                return node.Value;
            }
            catch (Exception)
            {
            }

            return default;
        }

        set
        {
            try
            {
                var node = _tree.GetNode(key);

                node.Value = value;
            }
            catch (Exception)
            {
                _tree.Add(key, value);
            }
        }
    }

    public TValue GetValue(TKey key)
    {
        return _tree.GetNode(key).Value;
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        try
        {
            value = _tree.GetNode(key).Value;
            return true;
        }
        catch (Exception)
        {
        }

        value = default;
        return false;
    }

    public void Add(TKey key, TValue value)
    {
        try
        {
            _tree.Add(key, value);
        }
        catch (Exception)
        {
        }
    }

    public CustomList<TKey> Keys
    {
        get
        {
            var keys = new CustomList<TKey>();

            _tree.InOrderTraversal(node => keys.Add(node.Key));

            return keys;
        }
    }

    public CustomList<TValue> Values
    {
        get
        {
            var values = new CustomList<TValue>();

            _tree.InOrderTraversal(node => values.Add(node.Value));

            return values;
        }
    }

    public bool ContainsKey(TKey key)
    {
        return this[key] != null;
    }
}