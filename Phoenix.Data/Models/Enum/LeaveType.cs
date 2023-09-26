using System;
using System.Collections.Generic;
using System.Text;

namespace Phoenix.Data.Models
{
    public static class LeaveType
    {
        public const int Annual = 1;
        public const int Compensatory = 2;
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
                default:
                    result = "Annual";
                    break;
            }
            return result;
        }
    }
}
