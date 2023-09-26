using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.ProjectManagement
{
    public class Timesheet
    {
        public string id { get; set; }
        public string job_id { get; set; }
        [DisplayName("Client")]
        public string company_id { get; set; }
        public string company_name { get; set; }
        [DisplayName("Job Name")]
        public string job_name { get; set; }
        [DisplayName("Non Job")]
        public string non_job_name { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Start Date")]
        public DateTime start_date { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("End Date")]
        public DateTime end_date { get; set; }
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Sunday { get; set; }
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Monday { get; set; }
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Tuesday { get; set; }
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Wednesday { get; set; }
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Thursday { get; set; }
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Friday { get; set; }
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Saturday { get; set; }

        public string Status { get; set; }

        public string ApprovalId { get; set; }
        [DisplayName("Non Job")]
        public string NonJobId { get; set; }
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

        [DisplayName("Start Date Weekly")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [DisplayName("End Date Weekly")]
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
