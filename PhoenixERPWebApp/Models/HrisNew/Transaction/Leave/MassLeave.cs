using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class MassLeave : HrisEntityBase
    {
        [DisplayName("Code")]
        public string Id { get; set; }

        [DisplayName("Year")]
        public int Year { get; set; }

        [DisplayName("Leave From")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime LeaveFrom { get; set; }

        [DisplayName("Leave To")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime LeaveTo { get; set; }

        [DisplayName("Total Days")]
        public int TotalDays { get; set; }

        [DisplayName("Except Employee")]
        public string ExceptEmployee { get; set; }

        [DisplayName("Notes")]
        public string Reason { get; set; }

        [DisplayName("Request By")]
        public string RequestBy { get; set; }

        [DisplayName("Request Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? RequestDate { get; set; }

        public List<MassLeaveDetail> MassLeaveDetails { get; set; }

        [DisplayName("Status")]
        public int? Status { get; set; }

        [DisplayName("Status")]
        public string StatusDesc { get; set; }

        [DisplayName("Remarks")]
        public string RemarkRejected { get; set; }
        public bool IsApprove { get; set; }

        public InfoEmployee requestEmployee { get; set; }

    }
}