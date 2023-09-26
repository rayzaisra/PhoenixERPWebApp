using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("revaluation_list", Schema = "fn")]
    public class RevaluationList : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("jv_number"), StringLength(50)]
        public string Code { get; set; }

        [MaxLength(50)]
        [Column("legal_entity_id"), StringLength(50)]
        public string LegalEntityId { get; set; }

        [MaxLength(50)]
        [Column("currency_id"), StringLength(50)]
        public string CurrencyId { get; set; }

        [Column("revaluation_date")]
        public DateTime? RevaluationDate { get; set; }

        [Column("exchange_rate")]
        public decimal? ExchangeRate { get; set; }

        [MaxLength(50)]
        [Column("status"), StringLength(50)]
        public string Status { get; set; }

        //For List
        [NotMapped]
        public string LegalEntityName { get; set; }
        [NotMapped]
        public string CurrencyName { get; set; }
        [NotMapped]
        public string CurrencyCode { get; set; }
        [NotMapped]
        public string CurrencySymbol { get; set; }

        //For Approval 
        [NotMapped]
        public string LogDescription { get; set; }
    }
}