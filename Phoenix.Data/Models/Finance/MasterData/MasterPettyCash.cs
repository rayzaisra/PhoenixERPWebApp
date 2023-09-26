using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("master_petty_cash", Schema = "fn")]
    public class MasterPettyCash : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [Column("currency_id")]
        public string CurrencyId { get; set; }

        [MaxLength(50)]
        [Column("chart_account_id"), StringLength(50)]
        public string ChartAccountId { get; set; }

        [MaxLength(50)]
        [Column("legal_entity_id"), StringLength(50)]
        public string LegalEntityId { get; set; }

        [Column("is_settle")]
        public bool IsSettle { get; set; }

        [Column("bank_account_number")]
        public string BankAccountNumber { get; set; }

        [MaxLength(50)]
        [Column("bank_account_name"), StringLength(50)]
        public string BankAccountName { get; set; }

        [MaxLength(50)]
        [Column("chart_account_name"), StringLength(50)]
        public string ChartAccountName { get; set; }

        [MaxLength(50)]
        [Column("chart_account_number"), StringLength(50)]
        public string ChartAccountNumber { get; set; }

        [MaxLength(50)]
        [Column(nameof(Description)), StringLength(50)]
        public string Description { get; set; }

        [MaxLength(50)]
        [Column(nameof(Status)), StringLength(50)]
        public string Status { get; set; }

        [Column("amount_insused")]
        public decimal? AmountInsused { get; set; }

        [Column("balance")]
        public decimal? Balance { get; set; }

        [MaxLength(50)]
        [Column("holder_user_id"), StringLength(50)]
        public string HolderUserId { get; set; }

        [MaxLength(50)]
        [Column("delegate_user_id"), StringLength(50)]
        public string DelegateUserId { get; set; }

        [NotMapped]
        public string LegalEntityName { get; set; }

        [NotMapped]
        public string CurrencyCode { get; set; }
    }
}