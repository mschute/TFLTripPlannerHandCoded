using TFLTripPlannerHandCoded;

namespace TFLTripPlanner
{
    public class HandCodedMinHeap
    {
        public Station[] Heap { get; set; }
        public int Size { get; set; }
        public Station Root => Heap[0];

        public HandCodedMinHeap(int capacity)
        {
            Heap = new Station[capacity];
            Size = 0;
        }

        public void Insert(Station station)
        {
            if (Size == Heap.Length)
            {
                IncreaseHeapSize();
            }

            // Insert the new station at the end of the heap
            Heap[Size] = station;
            Size++;

            // Move the new station up to its correct position
            // First check if the new station is smaller than its parent
            // If it is, swap the new station with its parent
            int position = Size - 1;
            MoveUp(position);
        }

        private void MoveUp(int position)
        {
            while (position > 0 && Heap[position].TimeFromStart < Heap[ParentIndex(position)].TimeFromStart)
            {
                Swap(position, ParentIndex(position));
                position = ParentIndex(position);
            }
        }

        public void MoveDown(int position)
        {
            int smallestChild;

            // Find the smallest child of the current station and swap them.
            // Do this until the current station is smaller than both of its children.
            while (HasLeftChild(position))
            {
                smallestChild = LeftChildIndex(position);
                if (HasRightChild(position) && Heap[RightChildIndex(position)].TimeFromStart <
                    Heap[LeftChildIndex(position)].TimeFromStart)
                {
                    smallestChild = RightChildIndex(position);
                }

                if (Heap[position].TimeFromStart < Heap[smallestChild].TimeFromStart)
                {
                    break;
                }

                Swap(position, smallestChild);
                position = smallestChild;
            }
        }

        public bool Has(Station station)
        {
            int position = 0;
            for (int i = 0; i < Size; i++)
            {
                if (Heap[i] == station)
                {
                    return true;
                }
            }

            return false;
        }

        public void Delete(Station station)
        {
            // Step 1: Find the station to be deleted
            int position = 0;
            for (int i = 0; i < Size; i++)
            {
                if (Heap[i] == station)
                {
                    position = i;
                    break;
                }
            }

            // Step 2: Delete the station and replace it with the bottom-most/right-most station
            Heap[position] = Heap[Size - 1];
            // Decrease the size because we've left a blank space.
            Size--;

            // Step 3: Move the station up or down to its correct position
            MoveDown(position);
        }

        private void IncreaseHeapSize()
        {
            Station[] newHeap = new Station[Heap.Length * 2];
            for (int i = 0; i < Heap.Length; i++)
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
            Station temp = Heap[index1];
            Heap[index1] = Heap[index2];
            Heap[index2] = temp;
        }
    }
}