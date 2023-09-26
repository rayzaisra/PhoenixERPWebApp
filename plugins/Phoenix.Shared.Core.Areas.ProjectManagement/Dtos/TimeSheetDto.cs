using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Dtos
{
    public class TimeSheetDto
    {
        public string id { get; set; }
        public string job_id { get; set; }
        public string company_id { get; set; }
        public string company_name { get; set; }
        public string job_name { get; set; }
        public string non_job_name { get; set; }
        public string NonJobId { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public DateTime Sunday { get; set; }
        public DateTime Monday { get; set; }
        public DateTime Tuesday { get; set; }
        public DateTime Wednesday { get; set; }
        public DateTime Thursday { get; set; }
        public DateTime Friday { get; set; }
        public DateTime Saturday { get; set; }
        public string UserId { get; set; }
        public string BusinessUnitId { get; set; }
    }

    public class TimesheetApprovalList
    {
        [DisplayName("Request Code")]
        public string TimesheetId { get; set; }

        [DisplayName("Requestor")]
        public string RequestorId { get; set; }

        [DisplayName("Requestor")]
        public string RequestorName { get; set; }

        [DisplayName("From")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [DisplayName("To")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        [DisplayName("Request Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? RequestDate { get; set; }

        [DisplayName("Status")]
        public int? StatusApprove { get; set; }

        [DisplayName("Status")]
        public string StatusDesc { get; set; }

        public bool IsApprove { get; set; }
        public string DetailLink { get; set; }
    }
}
