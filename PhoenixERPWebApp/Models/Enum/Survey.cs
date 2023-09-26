using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models
{
    public static class Survey
    {
        public const int StronglyDisagree = 1;
        public const int Disagree = 2;
        public const int Agree = 3;
        public const int StronglyAgree = 4;

        public static string SurveyName(int value)
        {
            string result = "";
            switch (value)
            {
                case Survey.StronglyDisagree:
                    result = "Strongly Disagree";
                    break;
                case Survey.Disagree:
                    result = "Disagree";
                    break;
                case Survey.Agree:
                    result = "Agree";
                    break;
                case Survey.StronglyAgree:
                    result = "Strongly Agree";
                    break;
                default:
                    result = "Strongly Disagree";
                    break;
            }
            return result;
        }
    }
}
