namespace TFLTripPlannerHandCoded;

public class DictionaryEntry
{
    private string _key;
    private object _value;
    
    private DictionaryEntry? _parent = null;
    private DictionaryEntry? _leftChild = null;
    private DictionaryEntry? _rightChild = null;

    public DictionaryEntry(string key, object value)
    {
        _key = key;
        _value = value;
        _parent = null;
    }

    public DictionaryEntry(string key, object value, DictionaryEntry? parent)
    {
        _key = key;
        _value = value;
        _parent = parent;
    }

    public void SetLeftChild(DictionaryEntry? newLeftChild)
    {
        _leftChild = newLeftChild;

        if (newLeftChild != null)
        {
            newLeftChild._parent = this;
        }
    }
    
    public void SetRightChild(DictionaryEntry? newRightChild)
    {
        _rightChild = newRightChild;
        
        if (newRightChild != null)
        {
            newRightChild._parent = this;
        }
    }

    public string GetKey()
    {
        return _key;
    }
    
    public object GetValue()
    {
        return _value;
    }

    public DictionaryEntry? GetParent()
    {
        return _parent;
    }

    public DictionaryEntry? GetLeftChild()
    {
        return _leftChild;
    }
    
    public DictionaryEntry? GetRightChild()
    {
        return _rightChild;
    }
}