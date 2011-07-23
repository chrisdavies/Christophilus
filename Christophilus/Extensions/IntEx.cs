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
            if (i > 10 && i < 20)
            {
                return i + "th";
            }

            switch (i % 10)
            {
                case 1:
                    return i + "st";
                case 2:
                    return i + "nd";
                case 3:
                    return i + "rd";
                default:
                    return i + "th";
            }
        }
    }
}