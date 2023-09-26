using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class AllowanceComponent : HrisEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Allowance Name")]
        public string SubComponentName { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Value (Rp)")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal Value { get; set; }

        [DisplayName("Grade")]
        public string JobGradeId { get; set; }

        [DisplayName("Grade")]
        public string JobGradeName { get; set; }

        [DisplayName("Percent (%)")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal Percent { get; set; }
        [DisplayName("Is Default")]
        public bool Is_Default { get; set; }

        [DisplayName("Is Default")]
        public string IsDefaultName { get { return (IsDefault ?? false) ? "Yes" : "No"; } }
        [DisplayName("Is Salary Report")]
        public bool? Is_Salary_Report { get; set; }

        [DisplayName("Is Salary Report")]
        public string IsSalaryReportName { get { return (Is_Salary_Report ?? false) ? "Yes" : "No"; } }

    }
}