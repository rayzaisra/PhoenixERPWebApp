using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models
{
    public static class BPJSType
    {
        public const int Additional = 1;
        public const int Deduction = 2;

        public static string BPJSTypeName(int value)
        {
            string result = "";
            switch (value)
            {
                case Additional:
                    result = "Additional";
                    break;
                case Deduction:
                    result = "Deduction";
                    break;
                default:
                    result = "Additional";
                    break;
            }
            return result;
        }
    }
}
