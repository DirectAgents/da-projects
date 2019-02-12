using System;
using System.Collections.Generic;
using System.Linq;

namespace CakeExtracter.Helpers
{
    public static class ExceptionHelper
    {
        /// <summary>
        /// The method returns all messages of inner exceptions from the original exception
        /// </summary>
        /// <param name="exception">Original exception</param>
        /// <returns></returns>
        public static string GetAllExceptionMessages(this Exception exception)
        {
            var messages = exception.FromHierarchy(ex => ex.InnerException)
                .Select(ex => ex.Message);
            return string.Join(Environment.NewLine, messages);
        }

        private static IEnumerable<TSource> FromHierarchy<TSource>(
            this TSource source,
            Func<TSource, TSource> nextItem,
            Func<TSource, bool> canContinue)
        {
            for (var current = source; canContinue(current); current = nextItem(current))
            {
                yield return current;
            }
        }

        private static IEnumerable<TSource> FromHierarchy<TSource>(
            this TSource source,
            Func<TSource, TSource> nextItem)
            where TSource : class
        {
            return FromHierarchy(source, nextItem, s => s != null);
        }
    }
}
