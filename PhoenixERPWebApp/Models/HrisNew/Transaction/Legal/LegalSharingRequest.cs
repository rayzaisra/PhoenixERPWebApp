using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class LegalSharingRequest : HrisEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Document Name")]
        public string LegalDocumentId { get; set; }

        [DisplayName("Category")]
        public string Category { get; set; }

        [DisplayName("Counterpart")]
        public string CounterpartId { get; set; }

        [DisplayName("Start Date Sharing")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? StartSharing { get; set; }

        [DisplayName("End Date Sharing")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EndSharing { get; set; }

        [DisplayName("Purpose")]
        public string Purpose { get; set; }
        
        public LegalDocument LegalDocument { get; set; }

        [DisplayName("Status")]
        public int? Status { get; set; }

        [DisplayName("Status")]
        public string StatusDesc { get; set; }

        [DisplayName("Remarks")]
        public string RemarkRejected { get; set; }
        public bool IsApprove { get; set; }

        [DisplayName("Request By")]
        public string RequestBy { get; set; }

        [DisplayName("Counterpart")]
        public string CounterpartName { get; set; }
    }
}