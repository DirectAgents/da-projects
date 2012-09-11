using Microsoft.Practices.EnterpriseLibrary.Logging;
using System;
namespace DirectAgents.Common
{
    public static class FileNameUtility
    {
        public static string NonExistant(string fileName)
        {
            string result = fileName;
            int count = 1;
            while (System.IO.File.Exists(result))
            {
                var split = fileName.Split('.');
                result = split[0] + "(" + count++ + ")." + split[1];
            }

            Logger.Write(result);
            return result;
        }

        public static string Existing(string fileName)
        {
            string result = fileName;

            if (!System.IO.File.Exists(fileName))
            {
                return fileName;
            }

            Func<string[]> split = () => fileName.Split('.');
            Func<string> left = () => split()[0];
            Func<string> right = () => split()[1];
            Func<int, string> path = i => left() + "(" + i + ")." + right();
            Func<string, bool> exists = s => System.IO.File.Exists(result);

            int number;
            for (number = 1; exists(path(number)); number++) ;

            result = path(number - 1);

            Logger.Write(result);
            return result;
        }
    }
}
