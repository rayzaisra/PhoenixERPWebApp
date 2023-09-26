using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class TmLegalDocumentRequest : EntityBase
    {
        [DisplayName("Legal Document Id")]
        public int LegalDocumentId { get; set; }

        [DisplayName("Category Id")]
        public int CategoryId { get; set; }

        [DisplayName("Counter Part")]
        public string CounterPart { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; }

        [DisplayName("Due Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DueDate { get; set; }

        [DisplayName("Remarks")]
        public string Remarks { get; set; }

        [DisplayName("Approval")]
        public string Approval { get; set; }
    }
}