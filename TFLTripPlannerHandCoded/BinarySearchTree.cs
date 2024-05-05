public class Node<TKey, TValue>
    where TKey : IComparable<TKey>
{
    public TKey Key;
    public TValue Value;
    public Node<TKey, TValue> Left;
    public Node<TKey, TValue> Right;
}

public class BinarySearchTree<TKey, TValue>
    where TKey : IComparable<TKey>
{
    private Node<TKey, TValue> _root = null!;

    public Node<TKey, TValue> GetNode(TKey key)
    {
        return GetNode(_root, key);
    }

    private Node<TKey, TValue> GetNode(Node<TKey, TValue> node, TKey key)
    {
        if (null == node)
        {
            throw new Exception($"Key '{key}' Not Found");
        }

        return node.Key.CompareTo(key) switch
        {
            0 => node,
            > 0 => GetNode(node.Left, key),
            _ => GetNode(node.Right, key)
        };
    }

    public void Add(TKey key, TValue value)
    {
        if (null == _root)
        {
            _root = new Node<TKey, TValue>
            {
                Key = key,
                Value = value
            };

            return;
        }

        Add(_root, key, value);
    }

    private static void Add(Node<TKey, TValue> node, TKey key, TValue value)
    {
        var comparison = node.Key.CompareTo(key);

        if (comparison == 0)
        {
            throw new Exception($"Key '{key}' Already Exists");
        }

        if (comparison > 0)
        {
            if (null == node.Left)
            {
                node.Left = new Node<TKey, TValue>
                {
                    Key = key,
                    Value = value
                };

                return;
            }

            Add(node.Left, key, value);

            return;
        }

        if (null == node.Right)
        {
            node.Right = new Node<TKey, TValue>
            {
                Key = key,
                Value = value
            };

            return;
        }

        Add(node.Right, key, value);
    }

    public void InOrderTraversal(Action<Node<TKey, TValue>> func)
    {
        InOrderTraversal(_root, func);
    }

    private static void InOrderTraversal(Node<TKey, TValue> node, Action<Node<TKey, TValue>> func)
    {
        if (node == null) return;

        InOrderTraversal(node.Left, func);
        func(node);
        InOrderTraversal(node.Right, func);
    }
}