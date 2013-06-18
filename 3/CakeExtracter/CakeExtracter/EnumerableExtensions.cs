using System.Collections.Generic;
using System.Linq;

namespace CakeExtracter
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<List<T>> InSetsOf<T>(this IEnumerable<T> source, int max)
        {
            var set = new List<T>(max);
            foreach (var item in source)
            {
                set.Add(item);
                if (set.Count == max)
                {
                    yield return set;
                    set = new List<T>(max);
                }
            }
            if (set.Any())
                yield return set;
        }

        public static IEnumerable<List<T>> InSetsOf<T>(this T[] source, int max)
        {
            return InSetsOf(source.AsEnumerable(), max);
        }
    }
}