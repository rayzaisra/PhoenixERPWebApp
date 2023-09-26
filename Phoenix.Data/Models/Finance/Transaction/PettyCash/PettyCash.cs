using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models.Finance
{
    [Table("petty_cash", Schema = "fn")]
    public class PettyCash : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("refference_no"), StringLength(50)]
        public string RefferenceNo { get; set; }

        [Column("settlement_date")]
        public DateTime? SettlementDate { get; set; }

        [MaxLength(50)]
        [Column("holder_id"), StringLength(50)]
        public string HolderId { get; set; }

        [MaxLength(50)]
        [Column("division_id"), StringLength(50)]
        public string DivisionId { get; set; }

        [Column("exchange_rate")]
        public decimal? ExchangeRate { get; set; }

        [MaxLength(450)]
        [Column(nameof(Remarks)), StringLength(450)]
        public string Remarks { get; set; }

        [Column("status")]
        public int Status { get; set; }

        [Column("master_petty_cash_amount")]
        public decimal? MasterPettyCashAmount { get; set; }

        [Column("total_credit_amount")]
        public decimal? TotalCreditAmount { get; set; }

        [Column("ending_balance")]
        public decimal? EndingBalance { get; set; }

        [Column("currency_id"), StringLength(50)]
        public string CurrencyId { get; set; }

        [Column("legal_entity_id"), StringLength(50)]
        public string LegalEntityId { get; set; }

        [Column("coa_id"), StringLength(50)]
        public string CoaId { get; set; }

        [Column("filemaster_id"), StringLength(8000)]
        public string FileMasterId { get; set; }

        [Column("account_number"), StringLength(50)]
        public string BankAccountNumber { get; set; }

        //For List Petty Cash
        [NotMapped]
        public string LegalEntityName { get; set; }

        [NotMapped]
        public string HolderName { get; set; }

        [NotMapped]
        public string HolderName2 { get; set; }

        //[NotMapped]
        //public string BankName { get; set; }

        [NotMapped]
        public string StatusName { get; set; }

        //=========== Start For Detail Forms ================   
        [NotMapped]
        public IFormFile File { get; set; }

        [NotMapped]
        public string FilemasterName { get; set; }

        [NotMapped]
        public string DivisionName { get; set; }

        [NotMapped]
        public List<PettyCashDetail> Detail { get; set; }


        //For Approval
        [NotMapped]
        public string LogDescription { get; set; }

        //=========== End For Detail Forms ================  
    }
}
