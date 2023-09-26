using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Enum
{
    public class GoodRequestStatus
    {
        public const int Open = 0;
        public const int CurrentApproval = 1;
        public const int WaitingApproval = 2;
        public const int Approved = 3;
        public const int Rejected = 4;
        public const int Completed = 5;
        public const int Received = 6;

        public static string StatusName(int? value)
        {
            string result = "";
            switch (value)
            {
                case CurrentApproval:
                    result = "Current Approval";
                    break;
                case WaitingApproval:
                    result = "Waiting Approval";
                    break;
                case Approved:
                    result = "Approved";
                    break;
                case Rejected:
                    result = "Rejected";
                    break;
                case Completed:
                    result = "Completed";
                    break;
                case Received:
                    result = "Received";
                    break;
                default:
                    result = "Open";
                    break;
            }
            return result;
        }
    }
}
