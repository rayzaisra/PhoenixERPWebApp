using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("journal_temp", Schema = "fn")]
    public class JournalTemp : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [Column("transaction_date")]
        public DateTime? TransactionDate { get; set; }

        [MaxLength(50)]
        [Column("reference_id"), StringLength(50)]
        public string ReferenceId { get; set; }

        [MaxLength(100)]
        [Column("reference_number"), StringLength(100)]
        public string ReferenceNumber { get; set; }

        [Column(nameof(Referencedate))]
        public DateTime? Referencedate { get; set; }

        //[MaxLength(50)]
        //[Column(nameof(Referenceto)), StringLength(50)]
        //public string Referenceto { get; set; }

        [Column(nameof(Description)), StringLength(2147483647)]
        public string Description { get; set; }

        [MaxLength(50)]
        [Column("default_currency_id"), StringLength(50)]
        public string DefaultCurrencyId { get; set; }

        [MaxLength(50)]
        [Column("currency_id"), StringLength(50)]
        public string CurrencyId { get; set; }

        //[Column(nameof(Qty))]
        //public decimal? Qty { get; set; }

        //[MaxLength(50)]
        //[Column(nameof(Unit)), StringLength(50)]
        //public string Unit { get; set; }

        [Column("exchange_rate")]
        public decimal? ExchangeRate { get; set; }

        //[Column(nameof(Price))]
        //public decimal? Price { get; set; }

        //[Column(nameof(Budgetdebit))]
        //public decimal? Budgetdebit { get; set; }

        //[Column(nameof(Budgetcredit))]
        //public decimal? Budgetcredit { get; set; }

        //[Column(nameof(Budgetgballance))]
        //public decimal? Budgetgballance { get; set; }

        //[Column(nameof(Beginningdebit))]
        //public decimal? Beginningdebit { get; set; }

        //[Column(nameof(Beginningcredit))]
        //public decimal? Beginningcredit { get; set; }

        //[Column(nameof(Beginningballance))]
        //public decimal? Beginningballance { get; set; }

        [Column("original_amount")]
        public decimal? OriginalAmount { get; set; }

        [Column(nameof(Debit))]
        public decimal? Debit { get; set; }

        [Column(nameof(Credit))]
        public decimal? Credit { get; set; }

        //[Column(nameof(Balance))]
        //public decimal? Balance { get; set; }

        //[Column(nameof(Endingdebet))]
        //public decimal? Endingdebet { get; set; }

        //[Column(nameof(Endingcredit))]
        //public decimal? Endingcredit { get; set; }

        //[Column(nameof(Endingbalance))]
        //public decimal? Endingbalance { get; set; }

        //[MaxLength(20)]
        //[Column(nameof(Rcondition)), StringLength(20)]
        //public string Rcondition { get; set; }

        //[Column(nameof(Flag))]
        //public bool? Flag { get; set; }

        //[Column(nameof(Fdate))]
        //public DateTime? Fdate { get; set; }

        //[Column(nameof(Ftime))]
        //public TimeSpan? Ftime { get; set; }

        [Column("pce_id")]
        public string PceId { get; set; }

        [Column("pca_id")]
        public string PcaId { get; set; }

        [Column("job_id")]
        public string JobId { get; set; }

        //[MaxLength(50)]
        //[Column("external_id"), StringLength(50)]
        //public string ExternalId { get; set; }

        //[MaxLength(50)]
        //[Column("internal_id"), StringLength(50)]
        //public string InternalId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("account_id"), StringLength(50)]
        public string AccountId { get; set; }

        //[MaxLength(50)]
        //[Column("invoice_id"), StringLength(50)]
        //public string InvoiceId { get; set; }

        [Column("po_id")]
        public string PoId { get; set; }

        [MaxLength(50)]
        [Column("type_of_expense_id"), StringLength(50)]
        public string TypeOfExpenseId { get; set; }

        [MaxLength(50)]
        [Column("mainservice_category_id"), StringLength(50)]
        public string MainserviceCategoryId { get; set; }

        [MaxLength(50)]
        [Column("shareservice_id"), StringLength(50)]
        public string ShareserviceId { get; set; }

        [Column("business_unit_id")]
        public string DivisiId { get; set; }

        [MaxLength(50)]
        [Column("legal_id"), StringLength(50)]
        public string LegalId { get; set; }

        [MaxLength(50)]
        [Column("affiliate_id"), StringLength(50)]
        public string AffiliateId { get; set; }

        //[MaxLength(50)]
        //[Column("is_posted"), StringLength(50)]
        //public string IsPosted { get; set; }

        //[MaxLength(50)]
        //[Column("account_code"), StringLength(50)]
        //public string AccountCode { get; set; }

        [MaxLength(50)]
        [Column("line_code"), StringLength(50)]
        public string LineCode { get; set; }


        //[MaxLength(1)]
        //[Column("is_job_closed"), StringLength(1)]
        //public string IsJobClosed { get; set; }

        //[MaxLength(50)]
        //[Column("pca_task_id"), StringLength(50)]
        //public string PcaTaskId { get; set; }

        [MaxLength(50)]
        [Column("trx_source"), StringLength(50)]
        public string TrxSource { get; set; }

        [MaxLength(50)]
        [Column("fixed_asset_id"), StringLength(50)]
        public string FixedAssetId { get; set; }

        [MaxLength(50)]
        [Column("task_id"), StringLength(50)]
        public string TaskId { get; set; }

        [MaxLength(50)]
        [Column("Item_type_id"), StringLength(50)]
        public string ItemTypeId { get; set; }
    }
}