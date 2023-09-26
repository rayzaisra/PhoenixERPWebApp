using System;
using System.Collections.Generic;
using System.Text;

namespace PhoenixERPWebApp.Models
{
    public static class LeaveType
    {
        public const int Annual = 1;
        public const int Compensatory = 2;
        public const int Clean = 3;
        public static string LeaveTypeName(int? value)
        {
            string result = "";
            switch (value)
            {
                case Annual:
                    result = "Annual";
                    break;
                case Compensatory:
                    result = "Compensatory";
                    break;
                case Clean:
                    result = "Clean";
                    break;
                default:
                    result = "Annual";
                    break;
            }
            return result;
        }
    }

    public static class ExludeLeaveType
    {
        public const int Day = 1;
        public const int Month = 2;
        public static string ExludeLeaveTypeName(int? value)
        {
            string result = "";
            switch (value)
            {
                case Day:
                    result = "Day";
                    break;
                case Month:
                    result = "Month";
                    break;
                default:
                    result = "Day";
                    break;
            }
            return result;
        }
    }
}
