using System;

namespace ClientPortal.Web
{
    public static class Extensions
    {
        public static String ToStringKMB(this Int32 number)
        {
            var format = "0";
            if (number >= 1000000000)
                format = "0,,,.#B";
            else if (number >= 1000000)
                format = "0,,.#M";
            else if (number >= 1000)
                format = "0,.#K";
            return number.ToString(format);
        }
        public static String ToStringKMB(this decimal number)
        {
            int numInt = (int)Math.Round(number, MidpointRounding.AwayFromZero);
            return numInt.ToStringKMB();
        }
    }
}