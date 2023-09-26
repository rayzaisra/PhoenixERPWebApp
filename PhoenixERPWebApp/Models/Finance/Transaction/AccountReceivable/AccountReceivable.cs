using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models
{
    public class AccountReceivable : FinanceEntityBase
    {
        [DisplayName("Invoice Number")]
        public string Id { get; set; }

        [DisplayName("AR Number")]
        public string AccountReceivableNumber { get; set; }

        [DisplayName("Legal Entity")]
        public string LegalEntityId { get; set; }

        [DisplayName("Bussiness Unit")]
        public string BusinessUnitId { get; set; }

        [DisplayName("Affiliation")]
        public string AffilitionId { get; set; }

        [DisplayName("Account")]
        public string AccountId { get; set; }

        [DisplayName("Brand")]
        public string BrandId { get; set; }

        [DisplayName("Job")]
        public string JobId { get; set; }

        [DisplayName("Invoice Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime TransactionDate { get; set; }

        //[DisplayName("Due Date")]
        //[DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DueDate { get; set; }

        [DisplayName("Currency")]
        public string CurrencyId { get; set; }

        [DisplayName("Amount")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Amount { get; set; }

        [DisplayName("Exchange Rate")]
        public decimal? ExchangeRate { get; set; }

        [DisplayName("Bank Acct Desination Id")]
        public string BankAccountDestination { get; set; }

        [DisplayName("Charges")]
        public decimal? Charges { get; set; }

        [DisplayName("Remarks")]
        public string Remarks { get; set; }

        [DisplayName("Debit / Kredit")]
        public string d_k { get; set; }

        [DisplayName("is_journal")]
        public string IsJournal { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; }


        [DisplayName("Pce Number")]
        public string PceCode { get; set; }

        [DisplayName("Account")]
        public string BankAccountDestinationName { get; set; }

        [DisplayName("Legal Enity")]
        public string LegalEntity { get; set; }

        [DisplayName("Client Brand")]
        public string ClientBrand { get; set; }

        [DisplayName("Client")]
        public string CompanyName { get; set; }

        [DisplayName("Currency")]
        public string CurrencyCode { get; set; }

        [DisplayName("Main Service Category")]
        public string JobType { get; set; }
        [DisplayName("Job Name")]
        public string JobName { get; set; }
        public string MultiPceId { get; set; }
        public string VatNo { get; set; }

        public string BusinessUnitName { get; set; }
        public DateTime? InvoiceDeliveryDate { get; set; }
        public string InvoiceDeliveryNotesId { get; set; }

        public string ClientId { get; set; }
        public string ClientName { get; set; }


        //public LegalEntity LegalEntity { get; set; }
        //public BusinessUnit BusinessUnit { get; set; }
        //public Affiliation Affiliation { get; set; }
        //public Account Account { get; set; }
        //public Company Company { get; set; }
        //public JobPe JobPe { get; set; }
        //public Currency Currency { get; set; }

        [DisplayName("Total Amount")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? TotalAmount { get; set; }

        [DisplayName("VAT Amount")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? VATAmount { get; set; }

        [DisplayName("Currency")]
        public string CurrencyName { get; set; }

        public string CurrencySymbol { get; set; }
        public string CurrencyIdDefault { get; set; }
        public string CurrencySymbolDefault { get; set; }
    }
}
