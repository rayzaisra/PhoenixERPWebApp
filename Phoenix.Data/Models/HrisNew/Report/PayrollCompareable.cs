using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    public class PayrollCompareable
    {
        [Key]
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
