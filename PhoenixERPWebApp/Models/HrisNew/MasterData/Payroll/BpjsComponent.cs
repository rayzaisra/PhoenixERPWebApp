using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class BpjsComponent : HrisEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Sub Component Name")]
        public string SubComponentName { get; set; }

        [DisplayName("Responsibility")]
        public int Responsibility { get; set; }

        [DisplayName("Value Employee(%)")]
        public decimal Value { get; set; }

        [DisplayName("Value Corporate(%)")]
        public decimal ValueCorporate { get; set; }

        [DisplayName("Max Limit")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal MaxLimit { get; set; }

        [DisplayName("Is Tax")]
        public bool? IsTax { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Type")]
        public int? Type { get; set; }

        [DisplayName("Tax")]
        public string TaxDesc { get { return (IsTax ?? false) ? "Yes" : "No"; } }

        [DisplayName("Responsibility")]
        public string ResponsibilityName { get { return ResponsibilityType.ResponsibilityTypeName(Responsibility); } }

        [DisplayName("Type")]
        public string TypeName { get { return BPJSType.BPJSTypeName(Type ?? 0); } }

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