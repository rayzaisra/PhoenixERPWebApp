using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("finance_period", Schema = "fn")]
    public class FinancePeriod : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("fiscal_year_id"), StringLength(50)]
        public string FiscalYearId { get; set; }

        [Required]
        [Column("finance_period_name"), StringLength(50)]
        public string FinancePeriodName { get; set; }

        [MaxLength(50)]
        [Column("finance_period_number"), StringLength(50)]
        public string FinancePeriodNumber { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("legal_entity_id"), StringLength(50)]
        public string LegalEntityId { get; set; }

        [Required]
        [Column("start_date")]
        public DateTime StartDate { get; set; }

        [Required]
        [Column("end_date")]
        public DateTime EndDate { get; set; }

        [Required]
        [MaxLength(50)]
        [Column(nameof(Status)), StringLength(50)]
        public string Status { get; set; }

        [Column("is_close")]
        public bool? IsClose { get; set; }

        [DisplayName("Ap")]
        public bool? Ap { get; set; }

        [DisplayName("Ar")]
        public bool? Ar { get; set; }

        [DisplayName("Cas")]
        public bool? Cas { get; set; }

        [DisplayName("Pcs")]
        public bool? Pcs { get; set; }

        [DisplayName("Bp")]
        public bool? Bp { get; set; }

        [DisplayName("Br")]
        public bool? Br { get; set; }

        [DisplayName("Je")]
        public bool? Je { get; set; }

        [DisplayName("Ja")]
        public bool? Ja { get; set; }

        [DisplayName("Amor")]
        public bool? Amor { get; set; }

        [DisplayName("Dep")]
        public bool? Dep { get; set; }

        [DisplayName("Sa")]
        public bool? Sa { get; set; }

        [DisplayName("Jc")]
        public bool? Jc { get; set; }

        [NotMapped]
        public string LegalEntityName { get; set; }
        [NotMapped]
        public string RemarksProcess { get; set; }
        [NotMapped]
        public int? StatusApproval { get; set; }
    }
}