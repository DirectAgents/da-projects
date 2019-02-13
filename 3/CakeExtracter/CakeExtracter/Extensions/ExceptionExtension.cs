using System;
using System.Collections.Generic;
using System.Linq;

namespace CakeExtracter.Extensions
{
    public static class ExceptionExtension
    {
        /// <summary>
        /// The method returns all messages of inner exceptions from the original exception
        /// </summary>
        /// <param name="exception">Original exception</param>
        /// <returns></returns>
        public static string GetAllExceptionMessages(this Exception exception)
        {
            var messages = GetInnerExceptionsList(exception).Select(ex => ex.Message);
            return string.Join(Environment.NewLine, messages);
        }
        
        private static IEnumerable<Exception> GetInnerExceptionsList(Exception source)
        {
            return FromHierarchy(source, ex => ex.InnerException, s => s != null);
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
    }
}
