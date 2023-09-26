using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class JobClosingDetail : FinanceEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Job Closing Header Id")]
        public string JobClosingHeaderId { get; set; }

        [DisplayName("Trx Code")]
        public string TrxCode { get; set; }

        [DisplayName("Journal Id")]
        public string JournalId { get; set; }

        [DisplayName("Reff Number")]
        public string ReffNumber { get; set; }

        [DisplayName("Reff Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ReffDate { get; set; }

        [DisplayName("Coa Id")]
        public string CoaId { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Debit")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Debit { get; set; }

        [DisplayName("Credit")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Credit { get; set; }
    }
}