using System;
using System.Collections.Generic;

namespace WS_4.Engine;

public sealed class EventQueue
{
    private readonly List<SimEvent> _heap = new();

    public int Count => _heap.Count;

    public void Enqueue(SimEvent simEvent)
    {
        _heap.Add(simEvent);
        HeapifyUp(_heap.Count - 1);
    }

    public SimEvent Dequeue()
    {
        if (_heap.Count == 0)
        {
            throw new InvalidOperationException("Event queue is empty.");
        }

        SimEvent root = _heap[0];
        SimEvent last = _heap[^1];
        _heap.RemoveAt(_heap.Count - 1);
        if (_heap.Count > 0)
        {
            _heap[0] = last;
            HeapifyDown(0);
        }

        return root;
    }

    private void HeapifyUp(int index)
    {
        while (index > 0)
        {
            int parent = (index - 1) / 2;
            if (Compare(_heap[index], _heap[parent]) >= 0)
            {
                break;
            }

            (_heap[index], _heap[parent]) = (_heap[parent], _heap[index]);
            index = parent;
        }
    }

    private void HeapifyDown(int index)
    {
        while (true)
        {
            int left = index * 2 + 1;
            int right = index * 2 + 2;
            int smallest = index;

            if (left < _heap.Count && Compare(_heap[left], _heap[smallest]) < 0)
            {
                smallest = left;
            }

            if (right < _heap.Count && Compare(_heap[right], _heap[smallest]) < 0)
            {
                smallest = right;
            }

            if (smallest == index)
            {
                break;
            }

            (_heap[index], _heap[smallest]) = (_heap[smallest], _heap[index]);
            index = smallest;
        }
    }

    private static int Compare(SimEvent a, SimEvent b)
    {
        int timeCompare = a.Time.CompareTo(b.Time);
        if (timeCompare != 0)
        {
            return timeCompare;
        }

        return a.Sequence.CompareTo(b.Sequence);
    }
}
