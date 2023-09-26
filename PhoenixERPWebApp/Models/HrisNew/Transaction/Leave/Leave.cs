using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class Leave : HrisEntityBase
    {
        [DisplayName("Request Code")]
        public string Id { get; set; }

        [DisplayName("Requestor")]
        public string EmployeeBasicInfoId { get; set; }

        [DisplayName("From")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [DisplayName("To")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        [DisplayName("Notes")]
        public string Reason { get; set; }

        [DisplayName("Total Days")]
        public int? TotalDays { get; set; }

        [DisplayName("Leave Request Type")]
        public string LeaveRequestTypeId { get; set; }

        [DisplayName("Assisted Job")]
        public string AssistedJob { get; set; }
        [DisplayName("Assisted Job")]
        public string AssistedJobName { get; set; }

        [DisplayName("Other Leave")]
        public string OtherLeave { get; set; }

        [DisplayName("Upload Files")]
        public string FileUpload { get; set; }

        [DisplayName("Files")]
        public string FileUploadName { get; set; }

        [DisplayName("Reached")]
        public string Reached { get; set; }

        [DisplayName("Request Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? RequestDate { get; set; }

        public LeaveRequestType LeaveRequestType { get; set; }
        public InfoEmployee requestEmployee { get; set; }
        public InfoEmployee infoEmployee { get; set; }

        [DisplayName("Status")]
        public int? Status { get; set; }

        [DisplayName("Status")]
        public string StatusDesc { get; set; }

        [DisplayName("Remarks")]
        public string RemarkRejected { get; set; }

        public bool IsApprove { get; set; }
        [DisplayName("Use Carry Over")]
        public bool? UseCarryOver { get; set; }
        [DisplayName("Use Current Year")]
        public bool? UseCurrentYear { get; set; }
    }
}