using System;
using System.Collections.Generic;
using System.Text;

namespace Phoenix.Data.Models.Enum
{
    public class EmployeeLoanStatus
    {
        public const int Open = 0;
        public const int CurrentApproval = 1;
        public const int WAITING_APPROVAL = 2;
        public const int Approved = 3;
        public const int Reject = 4;

        public static string StatusName(int? value)
        {
            string result = "";
            switch (value)
            {
                case Open:
                    result = "Open";
                    break;
                case CurrentApproval:
                    result = "Current Approval";
                    break;
                case WAITING_APPROVAL:
                    result = "Waiting Approval";
                    break;
                case Approved:
                    result = "Approved";
                    break;
                case Reject:
                    result = "Rejected";
                    break;
                default:
                    result = "Open";
                    break;
            }
            return result;
        }
    }
}
