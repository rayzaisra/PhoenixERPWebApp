using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phoenix.Data.Models
{
    public static class SalaryComponentTypeAdditional
    {
        public const int SalaryComponent = 1;
        public const int Allowance = 2;
        public const int BPJS = 3;

        public static string SalaryComponentTypeAdditionalName(int value)
        {
            string result = "";
            switch (value)
            {
                case SalaryComponent:
                    result = "Salary Component";
                    break;
                case Allowance:
                    result = "Allowance";
                    break;
                case BPJS:
                    result = "BPJS";
                    break;
                default:
                    result = "Salary Component";
                    break;
            }
            return result;
        }
    }

    public static class SalaryComponentTypeDeduction
    {
        public const int BPJS = 1;
        public const int Deduction = 2;

        public static string SalaryComponentTypeDeductionName(int value)
        {
            string result = "";
            switch (value)
            {
                case BPJS:
                    result = "BPJS";
                    break;
                case Deduction:
                    result = "Deduction";
                    break;
                default:
                    result = "BPJS";
                    break;
            }
            return result;
        }
    }
}
