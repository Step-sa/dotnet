using System;
using System.Collections;
using System.Collections.Generic;

namespace ArtGalleryApp
{
    public class GalleryCollection<T> : IEnumerable<T> where T : class
    {
        private readonly T[] _items;

        public GalleryCollection(int capacity)
        {
            _items = new T[capacity];
        }
        
        public int Count { get; private set; }
        public int Capacity => _items.Length;

        public bool TryAdd(T item)
        {
            if (Count == Capacity)
                return false;

            _items[Count] = item;
            Count++;
            return true;
        }

        // № 3
        public int CompareAt(int firstIndex, int secondIndex, Comparison<T> comparison)
        {
            return comparison(_items[firstIndex], _items[secondIndex]);
        }

        public void Sort(Comparison<T> comparison)
        {
            // № 3
            Array.Sort(_items, 0, Count, Comparer<T>.Create(comparison));
        }

        public void ForEach(Action<T> action)
        {
            foreach (T item in this)
                action(item);
        }

        public IEnumerator<T> GetEnumerator()
        {

            for (int i = 0; i < Count; i++)
                yield return _items[i];
        }


        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
