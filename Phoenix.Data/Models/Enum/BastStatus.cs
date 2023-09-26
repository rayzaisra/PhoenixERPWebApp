using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phoenix.Data.Models.Enum
{
    public class BastStatus
    {
        public const string Open = "1";
        public const string Closed = "2";       

        public static string StatusName(string value)
        {
            string result = "";
            switch (value)
            {
                case Open:
                    result = "Open";
                    break;
                case Closed:
                    result = "Closed";
                    break;
                default:
                    result = "Open";
                    break;
            }
            return result;
        }
    }
}
