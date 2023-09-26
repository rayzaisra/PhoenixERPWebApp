using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class SalaryMainComponent : HrisEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Sub Component Name")]
        public string SubComponentName { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Is Tax Calculate")]
        public bool? IsTaxCalculate { get; set; }

        [DisplayName("Is Salary")]
        public bool? IsSalary { get; set; }

        [DisplayName("Is Prorate Calculate")]
        public bool? IsProrateCalculate { get; set; }

        public string TaxCalculate { get { return (IsTaxCalculate ?? false) ? "Yes" : "No"; } }
        public string Salary { get { return (IsSalary ?? false) ? "Yes" : "No"; } }
        public string ProrateCalculate { get { return (IsProrateCalculate ?? false) ? "Yes" : "No"; } }
        [DisplayName("Is Salary Report")]
        public bool? Is_Salary_Report { get; set; }

        [DisplayName("Is Salary Report")]
        public string IsSalaryReportName { get { return (Is_Salary_Report ?? false) ? "Yes" : "No"; } }
    }
}