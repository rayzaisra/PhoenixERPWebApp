using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phoenix.Data.Models.Enum
{
    public class GoodRequestStatus
    {   
        public const int Open = 0;
        public const int CurrentApproval = 1;
        public const int WaitingApproval = 2;
        public const int Approved = 3;
        public const int Rejected = 4;
        public const int Completed = 5;
        public static string GoodRequestReturn = "12029293902abs9192";
        public static string GoodRequestNoReturn = "120029292988absu822";
        public static string GoodRequestMoving= "12029293902abs9193";
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
                default:
                    result = "Open";
                    break;
            }
            return result;
        }
    }
}
