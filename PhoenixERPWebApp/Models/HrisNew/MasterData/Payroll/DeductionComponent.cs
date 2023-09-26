using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class DeductionComponent : HrisEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Deducation Name")]
        public string SubComponentName { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }
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