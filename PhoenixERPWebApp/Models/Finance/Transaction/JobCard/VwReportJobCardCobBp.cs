using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models
{
    public class VwReportJobCardCobBp
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Date { get; set; }

        [DisplayName("Cv Number")]
        public string cv_number { get; set; }

        [DisplayName("Vendor Name")]
        public string vendor_name { get; set; }

        [DisplayName("Invoice Number")]
        public string invoice_number { get; set; }

        [DisplayName("Ap Number")]
        public string ap_number { get; set; }

        [DisplayName("Amount")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Amount { get; set; }

        [DisplayName("Paid")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Paid { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; }
    }
}
