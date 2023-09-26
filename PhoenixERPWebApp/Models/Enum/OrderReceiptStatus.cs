using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Enum
{
    public class OrderReceiptStatus
    {
        public const int Open = 0;
        public const int Delivered = 1;       

        public static string StatusName(int? value)
        {
            string result = "";
            switch (value)
            {
                case Delivered:
                    result = "Delivered";
                    break;
                default:
                    result = "Open";
                    break;
            }
            return result;
        }
    }
}
