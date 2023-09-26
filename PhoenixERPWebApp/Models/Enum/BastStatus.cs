using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Enum
{
    public class BastStatus
    {
        public const string Open = "1";
        public const string Closed = "2";
        public const string Rejected = "3";
        public static string StatusName(string value)
        {
            string result = "";
            switch (value)
            {
                case Open:
                    result = "Waiting Approval";
                    break;
                case Closed:
                    result = "Approoved";
                    break;
                case Rejected:
                    result = "Rejected";
                    break;
                default:
                    result = "Waiting Approval";
                    break;
            }
            return result;
        }
    }
}
