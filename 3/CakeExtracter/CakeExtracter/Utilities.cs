using System;
using System.Linq;

namespace CakeExtracter
{
    public static class Utilities
    {
        public static T Retry<T>(Func<T> action, int maxAttempts, int pauseMilliseconds, params Type[] retryWhenCaught)
        {
            int attempt = 1;
            while (true)
            {
                try
                {
                    Console.WriteLine("Attempt {0}..", attempt);
                    return action();
                }
                catch (Exception ex)
                {
                    if (retryWhenCaught.Any(c => c == ex.GetType()))
                    {
                        Console.WriteLine("Caught {0}: {1}", ex.GetType().Name, ex.Message);

                        attempt += 1;
                        if (attempt < maxAttempts)
                        {
                            Console.WriteLine("Pausing for {0} milliseconds..", pauseMilliseconds);
                            continue;
                        }
                        throw new Exception("max attempts exceeded");
                    }
                    throw;
                }
            }
        }
    }
}