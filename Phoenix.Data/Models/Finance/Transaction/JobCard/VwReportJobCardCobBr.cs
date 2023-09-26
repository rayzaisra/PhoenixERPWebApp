using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models
{
    public class VwReportJobCardCobBr
    {
        public string Id { get; set; }
        public DateTime? Date { get; set; }
        public string cr_number { get; set; }
        public string ar_number { get; set; }
        public decimal? invoice_amount { get; set; }
        public decimal? amount_paid { get; set; }
        public decimal? Pph { get; set; }
        public string Status { get; set; }
    }
}