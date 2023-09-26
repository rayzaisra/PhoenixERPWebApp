using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class RotationSubcomponentDeduction : HrisEntityBase
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
        public decimal? DeductionPercent { get; set; }

        [Required]
        [DisplayName("Amount")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? DeductionAmount { get; set; }

        [DisplayName("Rotation ID")]
        public string RotationId { get; set; }

        [DisplayName("Type")]
        public int? Type { get; set; }

        //Not Mapped
        [DisplayName("Salary Component")]
        public string SalaryComponentName { get { return SalaryComponentTypeDeduction.SalaryComponentTypeDeductionName(SalaryComponent ?? 0); } set { } }

        [DisplayName("Component Type")]
        public string ComponentTypeName { get; set; }
        public bool IsBasicSalary { get; set; }
    }
}