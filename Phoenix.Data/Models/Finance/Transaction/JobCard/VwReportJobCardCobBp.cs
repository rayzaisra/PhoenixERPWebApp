using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models
{
    public class VwReportJobCardCobBp
    {
        public string Id { get; set; }
        public DateTime? Date { get; set; }
        public string cv_number { get; set; }
        public string vendor_name { get; set; }
        public string invoice_number { get; set; }
        public string ap_number { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Paid { get; set; }
        public string Status { get; set; }
    }
}