using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models
{
    public class VwReportJobCardCobBr
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Date { get; set; }

        [DisplayName("CR Number")]
        public string cr_number { get; set; }

        [DisplayName("AR Number")]
        public string ar_number { get; set; }

        [DisplayName("Invoice Amount")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? invoice_amount { get; set; }

        [DisplayName("Amount Paid")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? amount_paid { get; set; }

        [DisplayName("PPH")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Pph { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; }
    }
}
