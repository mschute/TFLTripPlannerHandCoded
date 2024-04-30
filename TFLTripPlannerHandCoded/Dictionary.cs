namespace TFLTripPlannerHandCoded;

public class Dictionary
{
    private DictionaryEntry? _dictionaryRoot = null;

    public object? FindEntryValue(string key)
    {
        var current = _dictionaryRoot;

        while (current != null)
        {
            var comparison = current.GetKey().CompareTo(key);
            if (comparison == 0)
            {
                return current.GetValue();
            }

            current = comparison > 0 ? current.GetLeftChild() : current.GetRightChild();
        }
        
        return null;
    }

    public void Add(string key, object value)
    {
        if (_dictionaryRoot == null)
        {
            _dictionaryRoot = new DictionaryEntry(key, value);
        }
        else
        {
            Add(_dictionaryRoot, key, value);
        }
    }

    private static void Add(DictionaryEntry current, string key, object value)
    {
        var comparison = current.GetKey().CompareTo(key);

        switch (comparison)
        {
            case > 0 when current.GetLeftChild() == null:
                current.SetLeftChild(new DictionaryEntry(key, value));
                break;
            case > 0:
                Add(current.GetLeftChild(), key, value);
                break;
            case < 0 when current.GetRightChild() == null:
                current.SetRightChild(new DictionaryEntry(key, value, current));
                break;
            case < 0:
                Add(current.GetRightChild(), key, value);
                break;
        }
    }
}