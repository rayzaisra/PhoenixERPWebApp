using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class PayrollCompareable
    {
        //[DisplayName("Employee Basic Info Id")]
        //public string EmployeeBasicInfoId { get; set; }

        //[DisplayName("Current")]
        //[DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        //public decimal? CurrentBasicSalary { get; set; }

        //[DisplayName("Current")]
        //[DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        //public decimal? CurrentThp { get; set; }

        //[DisplayName("1st")]
        //[DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        //public decimal? OneMonthBeforeBasicSalary { get; set; }

        //[DisplayName("1st")]
        //[DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        //public decimal? OneMonthBeforeThp { get; set; }

        //[DisplayName("2nd")]
        //[DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        //public decimal? TwoMonthBeforeBasicSalary { get; set; }

        //[DisplayName("2nd")]
        //[DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        //public decimal? TwoMonthBeforeThp { get; set; }

        //[DisplayName("Employee Name")]
        //public string NameEmployee { get; set; }

        //[DisplayName("Current")]
        //[DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        //public string StringCurrentBasicSalary { get; set; }

        //[DisplayName("Current")]
        //[DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        //public string StringCurrentThp { get; set; }

        //[DisplayName("1st")]
        //[DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        //public string StringOneMonthBeforeBasicSalary { get; set; }

        //[DisplayName("1st")]
        //[DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        //public string StringOneMonthBeforeThp { get; set; }

        //[DisplayName("2nd")]
        //[DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        //public string StringTwoMonthBeforeBasicSalary { get; set; }

        //[DisplayName("2nd")]
        //[DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        //public string StringTwoMonthBeforeThp { get; set; }

        public string EmployeeBasicInfoId { get; set; }
        public decimal? CurrentBasicSalary { get; set; }
        public decimal? CurrentThp { get; set; }
        public decimal? OneMonthBeforeBasicSalary { get; set; }
        public decimal? OneMonthBeforeThp { get; set; }
        public decimal? TwoMonthBeforeBasicSalary { get; set; }
        public decimal? TwoMonthBeforeThp { get; set; }
        public string StringCurrentBasicSalary { get; set; }
        public string StringCurrentThp { get; set; }
        public string StringOneMonthBeforeBasicSalary { get; set; }
        public string StringOneMonthBeforeThp { get; set; }
        public string StringTwoMonthBeforeBasicSalary { get; set; }
        public string StringTwoMonthBeforeThp { get; set; }
        public string NameEmployee { get; set; }
    }
}
