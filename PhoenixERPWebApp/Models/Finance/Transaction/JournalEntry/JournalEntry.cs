using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class JournalEntry : FinanceEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Reference No")]
        public string Code { get; set; }

        [DisplayName("Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? JvDate { get; set; }

        [DisplayName("Currency")]
        public string CurrencyId { get; set; }

        [DisplayName("Exchange Rate")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? ExchangeRate { get; set; }

        //[DisplayName("Reference No")]
        //public string JvReferenceNo { get; set; }

        [DisplayName("Description")]
        public string JvDescription { get; set; }

        [DisplayName("Legal Entity")]
        public string LegalEntityId { get; set; }

        [DisplayName("Status")]
        public bool? IsDraft { get; set; }

        //Not Mapped
        public List<JournalEntryDetail> JournalEntryDetails { get; set; }

        [DisplayName("Legal Entity")]
        public string LegalEntityName { get; set; }
        public DateTime? JvDateEnd { get; set; }
    }
}