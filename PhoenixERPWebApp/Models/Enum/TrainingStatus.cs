using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Enum
{

    public class TrainingStatus
    {
        public const string Open = "0";
        public const string WaitingBondingApproval = "1";
        public const string WaitingApproval = "2";
        public const string Approved = "3";
        public const string Reject = "4";
        public const string Training = "5";
        public const string SharingSeassion = "6";


        public static string GetStatus(string value)
        {
            string result = "";
            switch (value)
            {
                case Open:
                    result = "Open";
                    break;
                case WaitingBondingApproval:
                    result = "Waiting Bonding";
                    break;
                case WaitingApproval:
                    result = "Waiting Approval";
                    break;
                case Approved:
                    result = "Approved";
                    break;
                case Reject:
                    result = "Reject";
                    break;
                case Training:
                    result = "Training";
                    break;
                case SharingSeassion:
                    result = "Sharing Seassion";
                    break;
                default:
                    result = "Open";
                    break;
            }
            return result;
        }

        public static int getIntVal(string value) {
            return int.Parse(value);
        }

    }
}
