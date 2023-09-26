using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models
{
    public class VwReportJobCardCobCa
    {
        public string pca_task_id { get; set; }
        public string task_name { get; set; }
        public string reff_number { get; set; }
        public DateTime Date { get; set; }
        public decimal? amount_request { get; set; }
        public decimal? amount_settlement { get; set; }
        public string Status { get; set; }
    }
}
