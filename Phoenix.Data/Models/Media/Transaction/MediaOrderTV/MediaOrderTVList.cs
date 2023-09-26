using System;
using System.Collections.Generic;
using System.Text;

namespace Phoenix.Data.Models.Media
{
    public class MediaOrderTVList
    {
        public string Id { get; set; }
        public string MediaOrderNo { get; set; }
        public string EmployeeBasicInfoId { get; set; }
        public string EmployeeBasicInfo_Name { get; set; }
        public string JobTitleId { get; set; }
        public string JobTitle_Name { get; set; }
        public decimal? TotalGrossValue { get; set; }
        public decimal? GrandTotal { get; set; }
        public decimal? TotalNetCost { get; set; }
        public decimal? Vat { get; set; }
        public string TmMediaOrderStatusId { get; set; }
        public string TmMediaOrderStatus_Name { get; set; }
    }
}
