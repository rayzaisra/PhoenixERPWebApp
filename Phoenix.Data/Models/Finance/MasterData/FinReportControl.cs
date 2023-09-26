using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("fin_report_control", Schema = "fn")]
    public class FinReportControl : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("item_code"), StringLength(50)]
        public string ItemCode { get; set; }

        [MaxLength(50)]
        [Column("coa_id"), StringLength(50)]
        public string CoaId { get; set; }

        [MaxLength(50)]
        [Column("d_k"), StringLength(50)]
        public string DK { get; set; }
    }

    public class VwFinReportControl
    {
        public string ItemCodeCurrencyGain { get; set; }
        public string CoaIdCurrencyGain { get; set; }
        public string ItemCodeCurrencyLoss { get; set; }
        public string CoaIdCurrencyLoss { get; set; }
        public string ItemCodeYTD { get; set; }
        public string CoaIdYTD { get; set; }
        public string ItemCodePAL { get; set; }
        public string CoaIdPAL { get; set; }
        public string ItemCodeREPY { get; set; }
        public string CoaIdREPY { get; set; }
        public string ItemCodeHistoricalBalance { get; set; }
        public string CoaIdHistoricalBalance { get; set; }
    }
}