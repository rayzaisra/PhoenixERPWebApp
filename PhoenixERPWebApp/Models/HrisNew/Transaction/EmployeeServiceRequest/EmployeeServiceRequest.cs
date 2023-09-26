using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class EmployeeServiceRequest : HrisEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Request Code")]
        public string RequestCode { get; set; }

        [DisplayName("Request Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? RequestDate { get; set; }

        [DisplayName("Status")]
        public int? Status { get; set; }

        [DisplayName("Employee Basic Info Id")]
        public string EmployeeBasicInfoId { get; set; }

        [DisplayName("Master Mapping Request Id")]
        public string MasterMappingRequestId { get; set; }

        [DisplayName("Due Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DueDate { get; set; }

        [DisplayName("Remarks")]
        public string Remarks { get; set; }

        [DisplayName("Fullfilment")]
        public string Fullfilment { get; set; }

        [DisplayName("Follow Up Remarks")]
        public string FollowUpRemarks { get; set; }

        [DisplayName("Date Close")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateClose { get; set; }

        [DisplayName("Comment")]
        public string Comment { get; set; }

        [DisplayName("Rating")]
        public string Rating { get; set; }

        [NotMapped]
        [DisplayName("Request By")]
        public string RequestBy { get; set; }

        [NotMapped]
        [DisplayName("Service Request Catalogue")]
        [Column(nameof(ServiceName))]
        public string ServiceName { get; set; }

        [NotMapped]
        [DisplayName("Division Name")]
        [Column(nameof(DivisionName))]
        public string DivisionName { get; set; }

        [NotMapped]
        [DisplayName("Date")]
        [Column(nameof(Date))]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public string Date { get; set; }

        [DisplayName("Status")]
        public string StatusDesc { get { return EmployeeServiceRequestStatus.StatusName(Status); } }

        public InfoEmployee requestEmployee { get; set; }

        public bool IsServiceReq { get; set; }

    }
}