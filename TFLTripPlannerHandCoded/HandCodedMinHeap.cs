namespace TFLTripPlannerHandCoded
{
    public class HandCodedMinHeap<T>
        where T : class, IComparable<T>
    {
        public T[] Heap { get; set; }
        public int Size { get; set; }
        public T Root => Heap[0];

        public HandCodedMinHeap(int capacity)
        {
            Heap = new T[capacity];
            Size = 0;
        }

        public void Insert(T obj)
        {
            if (Size == Heap.Length)
            {
                IncreaseHeapSize();
            }

            // Insert the new station at the end of the heap
            Heap[Size] = obj;
            Size++;

            // Move the new station up to its correct position
            // First check if the new station is smaller than its parent
            // If it is, swap the new station with its parent
            int position = Size - 1;
            MoveUp(position);
        }

        private void MoveUp(int position)
        {
            while (position > 0 && Heap[position].CompareTo(Heap[ParentIndex(position)]) < 0)
            {
                Swap(position, ParentIndex(position));
                position = ParentIndex(position);
            }
        }

        public void MoveDown(int position)
        {
            // Find the smallest child of the current station and swap them.
            // Do this until the current station is smaller than both of its children.
            while (HasLeftChild(position))
            {
                var smallestChild = LeftChildIndex(position);

                if (HasRightChild(position) &&
                    Heap[RightChildIndex(position)].CompareTo(Heap[LeftChildIndex(position)]) < 0)
                {
                    smallestChild = RightChildIndex(position);
                }

                if (Heap[position].CompareTo(Heap[smallestChild]) < 0)
                {
                    break;
                }

                Swap(position, smallestChild);
                position = smallestChild;
            }
        }

        public bool Has(T obj)
        {
            for (var i = 0; i < Size; i++)
            {
                if (Heap[i] == obj)
                {
                    return true;
                }
            }

            return false;
        }

        public void Delete(T obj)
        {
            // Step 1: Find the station to be deleted
            var position = 0;
            // Step 2: Delete the station and replace it with the bottom-most/right-most station
            Heap[position] = Heap[Size - 1];
            // Decrease the size because we've left a blank space.
            Size--;

            // Step 3: Move the station up or down to its correct position
            MoveDown(position);
        }

        private void IncreaseHeapSize()
        {
            var newHeap = new T[Heap.Length * 2];
            for (var i = 0; i < Heap.Length; i++)
            {
                newHeap[i] = Heap[i];
            }

            Heap = newHeap;
        }

        private int ParentIndex(int position)
        {
            return (position - 1) / 2;
        }

        private int LeftChildIndex(int position)
        {
            return 2 * position + 1;
        }

        private int RightChildIndex(int position)
        {
            return 2 * position + 2;
        }

        private bool HasLeftChild(int position)
        {
            return LeftChildIndex(position) < Size;
        }

        private bool HasRightChild(int position)
        {
            return RightChildIndex(position) < Size;
        }

        private void Swap(int index1, int index2)
        {
            T temp = Heap[index1];
            Heap[index1] = Heap[index2];
            Heap[index2] = temp;
        }
    }
}