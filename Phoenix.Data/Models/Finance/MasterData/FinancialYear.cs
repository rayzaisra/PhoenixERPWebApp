using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("financial_year", Schema = "fn")]
    public class FinancialYear : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("legal_entity_id"), StringLength(50)]
        public string LegalEntityId { get; set; }

        [MaxLength(50)]
        [Column("financial_year_number"), StringLength(50)]
        public string FinanceYearNumber { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("financial_year_name"), StringLength(50)]
        public string FinancialYearName { get; set; }

        [Required]
        [Column("start_date")]
        public DateTime StartDate { get; set; }

        [Required]
        [Column("end_date")]
        public DateTime EndDate { get; set; }

        [Required]
        [DisplayName("Status")]
        public string Status { get; set; }

        [Column("is_close")]
        public bool? IsClose { get; set; }

        [NotMapped]
        public string LegalEntityName { get; set; }
        [NotMapped]
        public string RemarksProcess { get; set; }
        [NotMapped]
        public int? StatusApproval { get; set; }
    }
}