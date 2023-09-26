using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class CASettlementJournalReverse : FinanceEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("JV number")]
        public string Code { get; set; }

        [DisplayName("Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? JvDate { get; set; }

        [DisplayName("Currency")]
        public string CurrencyId { get; set; }

        [DisplayName("Exchange Rate")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? ExchangeRate { get; set; }

        [DisplayName("Reference No")]
        public string JvReferenceNo { get; set; }

        [DisplayName("Description")]
        public string JvDescription { get; set; }

        [DisplayName("Legal Entity")]
        public string LegalEntityId { get; set; }

        [DisplayName("Business Unit")]
        public string BusinessUnitId { get; set; }

        [DisplayName("Affiliation")]
        public string AffiliationId { get; set; }

        [DisplayName("Amount")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Amount { get; set; }

        [DisplayName("JOB ID")]
        public string JobId { get; set; }

        [DisplayName("Account Payable")]
        public string ReceivableAccount { get; set; }

        [DisplayName("Reverse Account")]
        public string WriteOffAccount { get; set; }

        [DisplayName("Tax Ammount")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? TaxAmount { get; set; }

        [DisplayName("Tax Account")]
        public string TaxAccount { get; set; }

        [DisplayName("Status")]
        public bool? IsDraft { get; set; }

        [DisplayName("PO Number")]
        public string PONumber { get; set; }

        [DisplayName("AP Number")]
        public string APNumber { get; set; }
    }
}