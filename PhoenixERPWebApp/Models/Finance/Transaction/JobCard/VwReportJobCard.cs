using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models
{
    public class VwReportJobCard
    {
        [DisplayName("PCE ID")]
        public string pce_id { get; set; }

        [DisplayName("PCE Task ID")]
        public string task_id { get; set; }

        [DisplayName("Task Name")]
        public string task_name { get; set; }

        [DisplayName("Price")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? pce_price { get; set; }

        [DisplayName("Billed")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Billed { get; set; }

        [DisplayName("Balance PCE")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? balance_pce { get; set; }

        [DisplayName("PCA Task ID")]
        public string pca_task_id { get; set; }

        [DisplayName("PCA Price")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? pca_price { get; set; }

        [DisplayName("Estimated PO")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? estimated_po { get; set; }

        [DisplayName("Actual")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Actual { get; set; }

        [DisplayName("Accrual Gain or Loss")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? accrual_gain_losses { get; set; }

        [DisplayName("Actual Gain or Loss")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? gain_losses { get; set; }
    }
}
