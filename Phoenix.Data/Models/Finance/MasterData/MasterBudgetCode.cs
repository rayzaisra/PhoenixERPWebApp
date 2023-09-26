using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("master_budget_code", Schema = "fn")]
    public class MasterBudgetCode : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("budget_code"), StringLength(50)]
        public string BudgetCode { get; set; }

        [MaxLength(50)]
        [Column("budget_name"), StringLength(50)]
        public string BudgetName { get; set; }

        [MaxLength(50)]
        [Column("coa_id"), StringLength(50)]
        public string CoaId { get; set; }

        [MaxLength(50)]
        [Column("coa_name"), StringLength(50)]
        public string CoaName { get; set; }

        [MaxLength(50)]
        [Column("hris_division_id"), StringLength(50)]
        public string HrisDivisionId { get; set; }

        [MaxLength(50)]
        [Column("budget_owner_id"), StringLength(50)]
        public string BugetOwnerId { get; set; }

        [MaxLength(50)]
        [Column(nameof(Source)), StringLength(50)]
        public string Source { get; set; }

        [NotMapped]
        public string CodeRec { get; set; }

        [NotMapped]
        public string HrisDivisionName { get; set; }

        [NotMapped]
        public string BugetOwnerName { get; set; }
    }
}