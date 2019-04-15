using System;

namespace CakeExtracter.Common
{
    public static class CommandLineUtils
    {
        public static string[] GetCurrentCommandLineParams()
        {
            return Environment.GetCommandLineArgs();
        }
    }
}
