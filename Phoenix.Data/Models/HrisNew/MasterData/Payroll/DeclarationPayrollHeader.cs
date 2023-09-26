using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("declaration_payroll_header", Schema = "hr")]
    public class DeclarationPayrollHeader : HrisEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("code"), StringLength(50)]
        public string Code { get; set; }

        [Column("month")]
        public int? Month { get; set; }
        [NotMapped]
        public string MonthName { get; set; }

        [Column("year")]
        public int? Year { get; set; }

        [Column("is_tmp")]
        public bool? IsTmp { get; set; }
        [Column("declaration_type")]
        public string DeclarationType { get; set; }
        [Column("sub_component_id")]
        public string SubComponentId { get; set; }
        [NotMapped]
        public string SubComponentName { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("status")]
        public int? Status { get; set; }
    }

    public class DeclarationPayrollDto
    {
        public DeclarationPayrollHeader Header { get; set; }
        public List<DeclarationPayrollDetail> Details { get; set; }
    }
}