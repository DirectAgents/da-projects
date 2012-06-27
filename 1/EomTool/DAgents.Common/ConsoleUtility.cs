using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAgents.Common
{
    public static class ConsoleUtility
    {
        public static void WaitForKey()
        {
            Console.Write("Press a key...");
            Console.ReadKey(true);
        }
    }
}
