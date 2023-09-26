using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class LegalDocumentRequest : HrisEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Counterpart")]
        public string EmployeeBasicInfoId { get; set; }

        [DisplayName("Legal Category")]
        public string LegalCategoryId { get; set; }


        [DisplayName("Due Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DueDate { get; set; }

        [DisplayName("Upload Document File (.pdf)")]
        public string LegalDocFile { get; set; }

        [DisplayName("Remarks")]
        public string Remarks { get; set; }

        public bool? SubmitUser { get; set; }

        public LegalCategory LegalCategory { get; set; }

        //Not Mapped
        [DisplayName("Request By")]
        public string RequestBy { get; set; }

        [DisplayName("Counterpart")]
        public string Counterpart { get; set; }

        [DisplayName("Status")]
        public int? Status { get; set; }

        [DisplayName("Status")]
        public string StatusDesc { get; set; }

        [DisplayName("Remarks")]
        public string RemarkRejected { get; set; }
        public bool IsApprove { get; set; }
        public string LegalDocFileName { get; set; }
    }
}