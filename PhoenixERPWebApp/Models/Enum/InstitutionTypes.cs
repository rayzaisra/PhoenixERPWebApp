using System;
using System.Collections.Generic;
using System.Text;

namespace PhoenixERPWebApp.Models
{
    public class InstitutionTypes
    {
        public const string University = "1";
        public const string Academy = "2";
        public const string Courses = "3";
        public const string Others = "4";

        public static string GetName(string value)
        {
            string result = "";
            switch (value)
            {
                case University:
                    result = "University";
                    break;
                case Academy:
                    result = "Academy";
                    break;
                case Courses:
                    result = "Courses";
                    break;
                case Others:
                    result = "Others";
                    break;
                default:
                    result = "Others";
                    break;
            }
            return result;
        }
    }
}
