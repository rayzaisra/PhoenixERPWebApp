using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class SalaryProcessComponent : HrisEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Employee Basic Info Id")]
        public string EmployeeBasicInfoId { get; set; }

        [DisplayName("Component Name")]
        public string ComponentName { get; set; }

        [DisplayName("Value")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? Value { get; set; }

        [DisplayName("Value Year")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? ValueYear { get; set; }

        [DisplayName("Type")]
        public string Type { get; set; }

        [DisplayName("Tax Id")]
        public long? TaxId { get; set; }

        [DisplayName("Salary Process Header Id")]
        public string SalaryProcessHeaderId { get; set; }
    }
}