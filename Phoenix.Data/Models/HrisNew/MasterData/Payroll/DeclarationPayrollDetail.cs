using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("declaration_payroll_detail", Schema = "hr")]
    public class DeclarationPayrollDetail : HrisEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("declaration_payroll_header_id"), StringLength(50)]
        public string DeclarationPayrollHeaderId { get; set; }
        [Column("employee_basic_info_id")]
        public string EmployeeBasicInfoId { get; set; }

        [Column("amount")]
        public double? Amount { get; set; }

        [NotMapped]
        public string EmployeeName { get; set; }

        [Column("status")]
        public int? Status { get; set; }
        [Column("remarks")]
        public string Remarks { get; set; }

        [NotMapped]
        public string LegalEntityId { get; set; }
        [NotMapped]
        public string LegalEntityName { get; set; }
    }
}