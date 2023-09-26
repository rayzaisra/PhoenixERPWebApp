using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phoenix.Data.Models
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
        public const Decimal TrainingBondingContraint = 15000000;

        public static string GetStatus(string value)
        {
            string result = "";
            switch (value)
            {
                case Open:
                    result = "Open";
                    break;
                case WaitingBondingApproval:
                    result = "WaitingBondingApproval";
                    break;
                case WaitingApproval:
                    result = "WaitingApproval";
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
                    result = "SharingSeassion";
                    break;
                default:
                    result = "Open";
                    break;
            }
            return result;
        }

    }
}
