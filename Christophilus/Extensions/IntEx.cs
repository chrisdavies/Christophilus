namespace Christophilus.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public static class IntEx
    {
        public static string Th(this int i)
        {
            switch (i)
            {
                case 1:
                    return "1st";
                case 2:
                    return "2nd";
                case 3:
                    return "3rd";
                default:
                    return i + "th";
            }
        }
    }
}