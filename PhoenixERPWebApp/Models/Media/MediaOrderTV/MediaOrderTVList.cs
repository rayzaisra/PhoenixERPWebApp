using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Media
{
    public class MediaOrderTVList : MediaEntityBase
    {
        public string Id { get; set; }
        [DisplayName("Request Id")]
        public string MediaOrderNo { get; set; }
        public string EmployeeBasicInfoId { get; set; }
        [DisplayName("Request By")]
        public string EmployeeBasicInfo_Name { get; set; }
        public string JobTitleId { get; set; }
        [DisplayName("Job Title")]
        public string JobTitle_Name { get; set; }
        [DisplayName("Gross Value")]
        public decimal? TotalGrossValue { get; set; }
        [DisplayName("Gross Paid")]
        public decimal? GrandTotal { get; set; }
        [DisplayName("Nett")]
        public decimal? TotalNetCost { get; set; }
        public decimal? Vat { get; set; }
        public string TmMediaOrderStatusId { get; set; }
        [DisplayName("Status")]
        public string TmMediaOrderStatus_Name { get; set; }

    }
}
