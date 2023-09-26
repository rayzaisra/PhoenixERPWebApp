using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models
{
    public class VwReportJobCardCobCa
    {
        [DisplayName("PCA Task ID")]
        public string pca_task_id { get; set; }

        [DisplayName("Task Name")]
        public string task_name { get; set; }

        [DisplayName("Reff Number")]
        public string reff_number { get; set; }

        [DisplayName("Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [DisplayName("Amount Request")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? amount_request { get; set; }

        [DisplayName("Amount Settlement")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? amount_settlement { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; }
    }
}
