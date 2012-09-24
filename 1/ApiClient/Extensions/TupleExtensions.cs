using System;
using System.Collections.Generic;

namespace Extensions
{
    public static class TupleExtensions
    {
        public static IEnumerable<Tuple<int, int>> InSetIndiciesOf(this Tuple<int, int> source, int size)
        {
            int left = source.Item1;
            int right = source.Item2 + 1;
            if (right < left)
            {
                throw new Exception("Left index must me smaller than right.");
            }
            do
            {
                int start = left;
                int dist = right - left;
                int take = dist > size ? size : dist;
                left += take;
                yield return Tuple.Create(start, take);
            } while (left != right);
        }
    }
}
