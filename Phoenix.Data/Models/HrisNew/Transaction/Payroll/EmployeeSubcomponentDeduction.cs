using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("employee_subcomponent_deduction", Schema = "hr")]
    public class EmployeeSubcomponentDeduction : HrisEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("employee_basic_info_id"), StringLength(50)]
        public string EmployeeBasicInfoId { get; set; }

        [Column("salary_component")]
        public int? SalaryComponent { get; set; }

        [MaxLength(50)]
        [Column("component_id"), StringLength(50)]
        public string ComponentId { get; set; }

        //[Column("deduction_percent")]
        //public decimal? Percent { get; set; }

        //[Column("deduction_amount")]
        //public decimal? Amount { get; set; }

        [Column("deduction_percent")]
        public string Percent { get; set; }

        [Column("deduction_amount")]
        public string Amount { get; set; }

        [NotMapped]
        public string ComponentTypeName { get; set; }
        [Column("is_encrypt")]
        public bool? IsEncrypt { get; set; }

        [NotMapped]
        public int? month { get; set; }
        [NotMapped]
        public int? year { get; set; }
        [NotMapped]
        public double? AmountDes { get; set; }
    }
}