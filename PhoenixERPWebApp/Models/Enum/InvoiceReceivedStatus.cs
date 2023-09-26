using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Enum
{
    public class InvoiceReceivedStatus
    {
        //public const string Draft = "1";
        //public const string ApproveFrontDesk = "2";
        //public const string ApproveAdmin = "3";
        //public const string Rejected = "4";
        //public const string TAP = "5";
        //public const string ConfirmJurnal = "6";
        //public const string TaxVerification = "7";
        //public const string AP = "8";

        public const string Open = "1";
        public const string Revised = "2";
        public const string Rejected = "3";
        public const string TAP = "4";
        public const string TAXTAP = "5";
        public const string OpenAP = "6";
        public const string Posted = "7";
        public const string Cancelled = "8";

        public static string StatusName(string value)
        {
            string result = "";
            switch (value)
            {
                case Open:
                    result = "Open";
                    break;
                case Revised:
                    result = "Revised";
                    break;
                case Rejected:
                    result = "Rejected";
                    break;
                case TAP:
                    result = "TAP";
                    break;
                case TAXTAP:
                    result = "Tax Review";
                    break;
                case OpenAP:
                    result = "Tax Verified";
                    break;
                case Posted:
                    result = "Posted";
                    break;
                case Cancelled:
                    result = "Cancelled";
                    break;
                default:
                    result = "Open";
                    break;
            }
            return result;
        }
    }

  
}
