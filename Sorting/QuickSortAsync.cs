using System;
using System.Threading.Tasks;
using Lab3.Extensions;

namespace Lab3.Sorting
{
    public class QuickSortAsync<T>
    {
        public static Func<T, T, int> ComparisonPredicate { get; set; } = default;

        public static void Sort(DispatchedObservableCollection<T> a, int lo = 0, int hi = -1)
        {
            long step = 0;
            long maxSteps = Convert.ToInt64(a.Count);
            qSort(a, step, maxSteps, lo, hi);
        }

        private static void qSort(DispatchedObservableCollection<T> a, long stepCount, long maxSteps, int lo = 0, int hi = -1)
        {
            if (stepCount++ >= maxSteps || a.Count == 1)
                return;

            if (hi == -1)
                hi = a.Count - 1;

            if (lo < hi)
            {
                int p = Partition(a, lo, hi);
                (int l, int r) = ThreeWayPartition(a, a[p], lo, hi);
                qSort(a, stepCount, maxSteps, lo, l - 1);
                qSort(a, stepCount, maxSteps, r, hi);
            }
        }

        private static int Partition(DispatchedObservableCollection<T> a, int lo, int hi)
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

        private static (int, int) ThreeWayPartition(DispatchedObservableCollection<T> a, T pivot, int lo, int hi)
        {
            int l = lo;
            int r = lo;
            int u = hi;

            while (r <= u)
            {
                int comparisonValue = ComparisonPredicate(a[r], pivot);

                if (comparisonValue < 0)
                {
                    a.Swap(l, r);
                    l++;
                    r++;
                }
                else if (comparisonValue > 0)
                {
                    a.Swap(r, u);
                    u--;
                }
                else
                {
                    r++;
                }
            }

            return (l, r);
        }
    }
}