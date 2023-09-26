using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models
{
    public class VwReportJobCardCobAr
    {
        [DisplayName("PCE ID")]
        public string pce_id { get; set; }

        [DisplayName("PCE Task ID")]
        public string task_id { get; set; }

        [DisplayName("Task Name")]
        public string task_name { get; set; }

        [DisplayName("AR Number")]
        public string ar_number { get; set; }

        [DisplayName("Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Date { get; set; }

        [DisplayName("Amount")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Amount { get; set; }

        [DisplayName("Sum Amount Billed")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? sum_amount_billed { get; set; }

        [DisplayName("Balance")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Balance { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; }

        public string id { get; set; }
        public string parent_id { get; set; }
    }
}
