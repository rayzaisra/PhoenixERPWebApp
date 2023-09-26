using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Enum
{
    public class TrainingStatusSubmitToFinance
    {
        public const string WaitingSubmitToFinance = "0";
        public const string Save = "1";
        public const string Submit = "5";


        public static string GetStatus(string value)
        {
            string result = "";
            switch (value)
            {
                case WaitingSubmitToFinance:
                    result = "Waiting Add Accomodation";
                    break;
                case Save:
                    result = "Waiting Complete Training";
                    break;
                case Submit:
                    result = "Complete Training";
                    break;
                default:
                    result = "Waiting Add Accomodation";
                    break;
            }
            return result;
        }
    }
}
