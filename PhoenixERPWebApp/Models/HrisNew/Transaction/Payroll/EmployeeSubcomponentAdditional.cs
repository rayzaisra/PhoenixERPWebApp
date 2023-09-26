using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class EmployeeSubcomponentAdditional : HrisEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Employee Basic Info Id")]
        public string AdditionalEmployeeBasicInfoId { get; set; }

        [Required]
        [DisplayName("Salary Component")]
        public int? AdditionalSalaryComponent { get; set; }

        [Required]
        [DisplayName("Component Type")]
        public string AdditionalComponentId { get; set; }

        [Required]
        [DisplayName("Percent Value(%)")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? AdditionalPercent { get; set; }

        [Required]
        [DisplayName("Amount")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? AdditionalAmount { get; set; }

        //[Required]
        //[DisplayName("Percent Value(%)")]

        //public string AdditionalPercent { get; set; }

        //[Required]
        //[DisplayName("Amount")]

        //public string AdditionalAmount { get; set; }

        //Not Mapped
        [DisplayName("Salary Component")]
        public string AdditionalSalaryComponentName { get { return SalaryComponentTypeAdditional.SalaryComponentTypeAdditionalName(AdditionalSalaryComponent ?? 0); } set { } }

        [DisplayName("Component Type")]
        public string AdditionalComponentTypeName { get; set; }

        public string EncryptAmount { get; set; }
    }
}