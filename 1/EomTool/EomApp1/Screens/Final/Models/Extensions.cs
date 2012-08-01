using System;
using System.Data.Objects;
using System.Linq;

namespace EomApp1.Screens.Final.Models
{
    public static class Extensions
    {
        public static void ForEach<T>(this ObjectSet<T> objectSet, Action<T> action) where T : class
        {
            objectSet.ToList().ForEach(c => action(c));
        }

        public static void ForEach<T>(this IQueryable<T> objectSet, Action<T> action) where T : class
        {
            objectSet.ToList().ForEach(c => action(c));
        }
    }
}
