using System;
using System.Collections.ObjectModel;
using Lab3.Extensions;

namespace Lab3.Sorting
{
    public static class QuickSort<T>
    {
        public static Func<T, T, int> ComparisonPredicate { get; set; } = default;

        public static void Sort(ObservableCollection<T> a, int lo = 0, int hi = -1)
        {
            if (hi == -1)
                hi = a.Count - 1;

            if (lo < hi)
            {
                int p = Partition(a, lo, hi);
                Sort(a, lo, p - 1);
                Sort(a, p + 1, hi);
            }
        }

        private static int Partition(ObservableCollection<T> a, int lo, int hi)
        {
            T pivot = a[hi];
            int i = lo;

            for (int j = lo; j < hi; j++)
            {
                if (ComparisonPredicate(a[j], pivot) < 0)
                {
                    a.Swap(i, j);
                    i++;
                }
            }

            a.Swap(i, hi);
            return i;
        }

    }
}