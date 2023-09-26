using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class TmLegalDocumentFullfilment : EntityBase
    {
        [DisplayName("Id")]
        public int Id { get; set; }

        [DisplayName("Category Id")]
        public int CategoryId { get; set; }

        [DisplayName("Counter Part")]
        public string CounterPart { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; }

        [DisplayName("Due Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DueDate { get; set; }

        [DisplayName("Remarks")]
        public string Remarks { get; set; }

        [DisplayName("Doc File")]
        public byte[] DocFile { get; set; }

        [DisplayName("Doc File Name")]
        public string DocFileName { get; set; }

        [DisplayName("Doc File Desc")]
        public string DocFileDesc { get; set; }
    }
}