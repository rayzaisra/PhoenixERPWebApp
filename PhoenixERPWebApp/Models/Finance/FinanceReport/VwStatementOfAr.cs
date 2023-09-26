using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PhoenixERPWebApp.Models
{
    public class VwStatementOfAr
    {
        public Guid? Id { get; set; }

        [DisplayName("Date Transaction")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime TransactionDate { get; set; }

        public string LegalId { get; set; }

        [DisplayName("Legal Entity")]
        public string LegalEntityName { get; set; }

        public string BusinessUnitId { get; set; }

        [DisplayName("Business Unit")]
        public string BusinessUnitName { get; set; }

        public string ClientId { get; set; }

        [DisplayName("Client Name")]
        public string ClientName { get; set; }

        public string MainserviceCategoryId { get; set; }

        [DisplayName("MSC")]
        public string MainserviceCategoryName { get; set; }

        public string JobId { get; set; }

        [DisplayName("JOB-ID Name")]
        public string JobName { get; set; }

        public string TypeOfExpenseId { get; set; }

        [DisplayName("PCE Type")]
        public string TypeOfExpense { get; set; }

        [DisplayName("INV No")]
        public string AccountReceivableNumber { get; set; }

        [DisplayName("Original Amount")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? OriginalAmount { get; set; }

        [DisplayName("Exhcange Rate")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? ExchangeRate { get; set; }

        [DisplayName("Debit")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? Debit { get; set; }

        [DisplayName("Credit")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? Credit { get; set; }

        [DisplayName("Balance")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? Balance { get; set; }
        public string CoaId { get; set; }
        public string CurrencyId { get; set; }

        [DisplayName("Currency")]
        public string CurrencyName { get; set; }
        public string CurrencySymbol { get; set; }
        public string CurrencySymbolExchange { get; set; }
    }

    public class VwStatementOfArFilter
    {
        [DisplayName("Chart Of Account")]
        public string CoaId { get; set; }

        [DisplayName("Client Name")]
        public string ClientId { get; set; }

        [DisplayName("Legal Entity")]
        public string LegalId { get; set; }

        [DisplayName("Begining Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? BeginDT { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Ending Date")]
        public DateTime? EndDT { get; set; }

        [DisplayName("Original Currency")]
        public string CurrencyId { get; set; }

        [DisplayName("MSC")]
        public string MainserviceCategoryId { get; set; }

        [DisplayName("JOB-ID Name")]
        public string JobId { get; set; }

        [DisplayName("PCE Type")]
        public string TypeOfExpenseId { get; set; }

        [DisplayName("Amount")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? OriginalAmount { get; set; }
    }
}
