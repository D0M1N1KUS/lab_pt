using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Lab3.Sorting;
using Lab3.Sorting.Enums;
using Lab3.ViewModel;

namespace Lab3.Extensions
{
    public static class ObservableCollectionExtensions
    {
        //public static void Sort<T, TKey>(this ObservableCollection<T> collection, Func<T, TKey> predicate, Direction sortingDirection = Direction.Ascending, Type itemType = null)
        //    where T : IComparable<T>, IEquatable<T>
        //{
        //    IEnumerable<T> sortedCollection =
        //        sortingDirection == Direction.Ascending
        //            ? collection.OrderBy(predicate)
        //            : collection.OrderByDescending(predicate);

        //    List<T> sortedList = itemType != null
        //        ? sortedCollection.Where(item => item.GetType() == itemType).ToList()
        //        : sortedCollection.ToList();

        //    int ptr = 0;
        //    while (ptr < sortedList.Count - 1)
        //    {
        //        if (!collection[ptr].Equals(sortedList[ptr]))
        //        {
        //            int idx = Search(collection, ptr + 1, sortedList[ptr]);
        //            collection.Move(idx, ptr);
        //        }

        //        ptr++;
        //    }
        //}

        //public static int Search<T>(ObservableCollection<T> collection, int startIndex, T other)
        //{
        //    for (int i = startIndex; i < collection.Count; i++)
        //    {
        //        if (other.Equals(collection[i]))
        //            return i;
        //    }

        //    return -1; // decide how to handle error case
        //}

        public static void Swap<T>(this DispatchedObservableCollection<T> current, int i, int j)
        {
            if (i >= current.Count || j >= current.Count || i < 0 || j < 0)
                throw new IndexOutOfRangeException(
                    $"One of the indexes ({i}, {j}) is out of range <0, {current.Count})");
            if (i == j)
                return;

            int max = Math.Max(i, j);
            int min = Math.Min(i, j);

            current.Move(min, max);
            current.Move(max - 1, min);
        }
    }
}