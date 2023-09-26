using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("salary_allocation_detail", Schema = "fn")]
    public class SalaryAllocationDetail : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("salary_allocation_id"), StringLength(50)]
        public string SalaryAllocationId { get; set; }

        [MaxLength(50)]
        [Column("coa_id"), StringLength(50)]
        public string CoaId { get; set; }

        [MaxLength(50)]
        [Column(nameof(Description)), StringLength(50)]
        public string Description { get; set; }

        [MaxLength(50)]
        [Column(nameof(Condition)), StringLength(50)]
        public string Condition { get; set; }

        [Column(nameof(Debit))]
        public decimal? Debit { get; set; }

        [Column(nameof(Credit))]
        public decimal? Credit { get; set; }

        [NotMapped]
        public string AccountName { get; set; }
        [NotMapped]
        public string CodeRec { get; set; }
    }
}