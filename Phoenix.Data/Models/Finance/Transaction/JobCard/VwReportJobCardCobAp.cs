using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models
{
    public class VwReportJobCardCobAp
    {
        public string pca_task_id { get; set; }
        public string task_name { get; set; }
        public string PO_Number { get; set; }
        public DateTime? po_date { get; set; }
        public decimal? po_amount { get; set; }
        public decimal? Vat { get; set; }
        public string invoice_number { get; set; }
        public DateTime? invoice_date { get; set; }
        public decimal? total_amount { get; set; }
        public decimal? total_vat { get; set; }
        public string Status { get; set; }
        public decimal? Balance { get; set; }
        public string id { get; set; }
        public string parent_id { get; set; }
    }
}
