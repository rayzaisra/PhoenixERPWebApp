using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models
{
    public class VwReportJobCardCobAr
    {
        public string pce_id { get; set; }
        public string task_id { get; set; }
        public string task_name { get; set; }
        public string ar_number { get; set; }
        public DateTime? Date { get; set; }
        public decimal? Amount { get; set; }
        public decimal? sum_amount_billed { get; set; }
        public decimal? Balance { get; set; }
        public string Status { get; set; }
        public string id { get; set; }
        public string parent_id { get; set; }
    }
}