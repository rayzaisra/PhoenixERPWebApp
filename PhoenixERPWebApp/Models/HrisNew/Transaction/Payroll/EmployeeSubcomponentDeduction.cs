using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class EmployeeSubcomponentDeduction : HrisEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Employee Basic Info Id")]
        public string EmployeeBasicInfoId { get; set; }

        [Required]
        [DisplayName("Salary Component")]
        public int? SalaryComponent { get; set; }

        [Required]
        [DisplayName("Component Type")]
        public string ComponentId { get; set; }

        [Required]
        [DisplayName("Percent Value(%)")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Percent { get; set; }

        [Required]
        [DisplayName("Amount")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? Amount { get; set; }

        //Not Mapped
        [DisplayName("Salary Component")]
        public string SalaryComponentName { get { return SalaryComponentTypeDeduction.SalaryComponentTypeDeductionName(SalaryComponent ?? 0); } set { } }

        [DisplayName("Component Type")]
        public string ComponentTypeName { get; set; }
        public string EncryptAmount { get; set; }
    }
}