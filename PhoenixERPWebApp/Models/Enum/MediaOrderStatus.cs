using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Enum
{
    public class MediaOrderStatus
    {
        public const string Draft = "0";
        public const string CurrentApproval = "1";
        public const string WaitingApproval = "2";
        public const string Approved = "3";
        public const string Reject = "4";
        public const string Finish = "5";
        public const string Published = "6";

        public static string StatusDescMedia(string value)
        {
            string result = "";
            switch (value)
            {
                case CurrentApproval:
                    result = "Current Approval";
                    break;
                case Draft:
                    result = "Open";
                    break;
                case WaitingApproval:
                    result = "Waiting Approval";
                    break;
                case Reject:
                    result = "Rejected";
                    break;
                case Approved:
                    result = "Approved";
                    break;
                case Finish:
                    result = "Finish";
                    break;
                case Published:
                    result = "Published";
                    break;
                default:
                    result = "Open";
                    break;
            }
            return result;
        }
    }
}
