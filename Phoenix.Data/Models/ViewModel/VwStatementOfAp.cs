using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models.ViewModel
{
    [Table("vw_statement_of_AP", Schema = "fn")]
    public class VwStatementOfAp
    {
        [Key]
        public Guid? Id { get; set; }

        [Column("account_payable_number")]
        public string AccountPayableNumber { get; set; }

        [Column("transaction_date")]
        public DateTime? TransactionDate { get; set; }

        [Column("legal_id")]
        public string LegalId { get; set; }

        [Column("legal_entity_name")]
        public string LegalEntityName { get; set; }

        [Column("bu_id")]
        public string BusinessUnitId { get; set; }

        [Column("bu_name")]
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

        [Column("job_name")]
        public string JobName { get; set; }

        [Column("type_of_expense_id")]
        public string TypeOfExpenseId { get; set; }

        [Column("type_of_expense")]
        public string TypeOfExpense { get; set; }

        [Column("original_amount")]
        public decimal? OriginalAmount { get; set; }

        [Column("exchange_rate")]
        public decimal? ExchangeRate { get; set; }

        [Column(nameof(Debit))]
        public decimal? Debit { get; set; }

        [Column(nameof(Credit))]
        public decimal? Credit { get; set; }

        [Column(nameof(Balance))]
        public decimal? Balance { get; set; }

        [Column("coa_id")]
        public string CoaId { get; set; }

        [Column("currency_id")]
        public string CurrencyId { get; set; }

        [Column("currency_name")]
        public string CurrencyName { get; set; }

        [Column("currency_symbol")]
        public string CurrencySymbol { get; set; }

        [Column("currency_symbol_exchange")]
        public string CurrencySymbolExchange { get; set; }

        [Column("po_number")]
        public string PONumber { get; set; }
    }

    public class VwStatementOfApFilter
    {
        public string CoaId { get; set; }
        public string VendorId { get; set; }
        public string LegalId { get; set; }
        public DateTime? BeginDT { get; set; }
        public DateTime? EndDT { get; set; }
        public string CurrencyId { get; set; }
        public string MainserviceCategoryId { get; set; }
        public string JobId { get; set; }
        public string TypeOfExpenseId { get; set; }
        public decimal? OriginalAmount { get; set; }
    }
}
