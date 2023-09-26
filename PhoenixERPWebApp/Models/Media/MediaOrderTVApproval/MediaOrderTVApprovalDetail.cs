using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Media
{
    public class MediaOrderTVApprovalDetail : MediaOrderTVDetail
    {
        [DisplayName("Request Code")]
        public string RequestCode { get; set; }
        [DisplayName("Request Date")]
        public DateTime? RequestDate { get; set; }
        [DisplayName("Request Status")]
        public string RequestStatus { get; set; }
        [DisplayName("Request By")]
        public string RequestBy { get; set; }
        [DisplayName("Job Title")]
        public string JobTitle { get; set; }

    }
}
