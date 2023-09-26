using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models
{
    public class VwReportJobCard
    {
        public string pce_id { get; set; }
        public string task_id { get; set; }
        public string task_name { get; set; }
        public decimal? pce_price { get; set; }
        public decimal? Billed { get; set; }
        public decimal? balance_pce { get; set; }
        public string pca_task_id { get; set; }
        public decimal? pca_price { get; set; }
        public decimal? estimated_po { get; set; }
        public decimal? Actual { get; set; }
        public decimal? accrual_gain_losses { get; set; }
        public decimal? gain_losses { get; set; }
    }
}
