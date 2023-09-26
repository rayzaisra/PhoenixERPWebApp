using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models
{
    public class VwReportJobCardCobAp
    {
        [DisplayName("PCA Task ID")]
        public string pca_task_id { get; set; }

        [DisplayName("Task Name")]
        public string task_name { get; set; }

        [DisplayName("PO Number")]
        public string PO_Number { get; set; }

        [DisplayName("Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? po_date { get; set; }

        [DisplayName("PO Amount")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? po_amount { get; set; }

        [DisplayName("VAT")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Vat { get; set; }

        [DisplayName("INV Number")]
        public string invoice_number { get; set; }

        [DisplayName("Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? invoice_date { get; set; }

        [DisplayName("Summ INV Ammount")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? total_amount { get; set; }

        [DisplayName("Summ VAT INV")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? total_vat { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; }

        [DisplayName("Balance")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Balance { get; set; }

        public string id { get; set; }
        public string parent_id { get; set; }
    }
}
