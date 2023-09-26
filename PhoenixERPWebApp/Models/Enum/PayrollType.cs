using System;
using System.Collections.Generic;
using System.Text;

namespace PhoenixERPWebApp.Models
{
    public static class ResponsibilityType
    {
        public const int Employee = 1;
        public const int Corporate = 2;
        public static string ResponsibilityTypeName(int? value)
        {
            string result = "";
            switch (value)
            {
                case Employee:
                    result = "Employee";
                    break;
                case Corporate:
                    result = "Corporate";
                    break;
                default:
                    result = "Employee";
                    break;
            }
            return result;
        }
    }

}
