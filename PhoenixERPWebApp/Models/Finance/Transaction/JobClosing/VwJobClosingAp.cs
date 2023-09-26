using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models
{
    public class VwJobClosingAp
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Job Id")]
        public string JobId { get; set; }

        [DisplayName("Reference Id")]
        public string ReferenceId { get; set; }

        [DisplayName("Reff Number")]
        public string ReffNumber { get; set; }

        [DisplayName("Account ID")]
        public string CoaId { get; set; }

        [DisplayName("Account ID")]
        public string CodeRec { get; set; }

        [DisplayName("Account Name")]
        public string Name5 { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Debit")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Debit { get; set; }

        [DisplayName("Credit")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Credit { get; set; }

        [DisplayName("Sharedservice Id")]
        public string SharedserviceId { get; set; }

        [DisplayName("Sharedservice Name")]
        public string SharedserviceName { get; set; }

        [DisplayName("Mainservice Category Id")]
        public string MainserviceCategoryId { get; set; }

        [DisplayName("Currency Id")]
        public string CurrencyId { get; set; }

        [DisplayName("Unit")]
        public string Unit { get; set; }

        [DisplayName("Exchange Rate")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? ExchangeRate { get; set; }

        [DisplayName("Legal Id")]
        public string LegalId { get; set; }

        [DisplayName("Affiliate Id")]
        public string AffiliateId { get; set; }

        [DisplayName("Business Unit Id")]
        public string BusinessUnitId { get; set; }

        [DisplayName("Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? TransactionDate { get; set; }
    }
}
