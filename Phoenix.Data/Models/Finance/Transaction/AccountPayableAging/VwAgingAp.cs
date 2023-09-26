using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models.ViewModel
{
    [Table("vw_aging_ap", Schema = "fn")]
    public class VwAgingAp
    {
        [Key]
        public string Id { get; set; }

        [Column("transaction_date")]
        public DateTime? TransactionDate { get; set; }

        [Column("invoice_received_date")]
        public DateTime? InvoiceReceivedDate { get; set; }

        [Column("legal_entity_id")]
        public string LegalEntityId { get; set; }

        [Column("legal_entity_name")]
        public string LegalEntityName { get; set; }

        [Column("business_unit_id")]
        public string BusinessUnitId { get; set; }

        [Column("unit_name")]
        public string BusinessUnitName { get; set; }

        [Column("vendor_id")]
        public string VendorId { get; set; }

        [Column("vendor_name")]
        public string VendorName { get; set; }

        [Column("mainservice_category_id")]
        public string MainserviceCategoryId { get; set; }

        [Column("mainservice_category_name")]
        public string MainserviceCategoryName { get; set; }

        [Column("job_id")]
        public string JobId { get; set; }

        [Column(nameof(Jobs))]
        public string Jobs { get; set; }

        [Column("type_of_expense_id")]
        public string TypeOfExpenseId { get; set; }

        [Column("type_of_expense_name")]
        public string TypeOfExpenseName { get; set; }

        [Column("account_payable_number")]
        public string APNumber { get; set; }

        [Column("currency_id")]
        public string CurrencyId { get; set; }

        [Column("currency_name")]
        public string CurrencyName { get; set; }

        [Column("currency_symbol")]
        public string CurrencySymbol { get; set; }

        [Column("ref_invoice_number")]
        public string RefInvoiceNumber { get; set; }

        [Column("invoice_number")]
        public string InvoiceNumber { get; set; }

        [Column("invoice_date")]
        public DateTime? InvoiceDate { get; set; }

        [Column("due_date")]
        public DateTime? DueDate { get; set; }

        [Column(nameof(Days))]
        public int? Days { get; set; }

        [Column("amount")]
        public decimal? Amount { get; set; }

        [Column("current")]
        public decimal? Current { get; set; }

        [Column("01-30")]
        public decimal? Value0130 { get; set; }

        [Column("31-60")]
        public decimal? Value3160 { get; set; }

        [Column("61-90")]
        public decimal? Value6190 { get; set; }

        [Column(">90")]
        public decimal? ValueMoreThan90 { get; set; }
    }

    public class VwAgingApFilter
    {
        public string VendorId { get; set; }
        public string LegalEntityId { get; set; }
        public string MainserviceCategoryId { get; set; }
        public string JobId { get; set; }
        public string TypeOfExpenseId { get; set; }
        public string CurrencyId { get; set; }
        public decimal? OriginalAmount { get; set; }
    }
}
