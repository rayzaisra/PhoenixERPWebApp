using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class JobClosingHeader : FinanceEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Job Closing Number")]
        public string RequestCode { get; set; }

        [DisplayName("Job Id")]
        public string JobId { get; set; }

        [DisplayName("Job Name")]
        public string JobName { get; set; }

        [DisplayName("Business Unit")]
        public string DivisiId { get; set; }

        [DisplayName("Legal Entity")]
        public string LegalEntityId { get; set; }

        [DisplayName("Client")]
        public string ClientId { get; set; }

        [DisplayName("Brand Name")]
        public string BrandId { get; set; }

        [DisplayName("Deadline")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Deadline { get; set; }

        [DisplayName("Job Status")]
        public string JobStatus { get; set; }

        [DisplayName("Finance Status")]
        public string FinanceStatus { get; set; }

        [DisplayName("Transaction Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? TransactionDate { get; set; }

        //NotMapped
        [DisplayName("Remarks")]
        public string RemarksProcess { get; set; }

        [DisplayName("Status")]
        public int? StatusApproval { get; set; }

        [DisplayName("Job Number")]
        public string JobNumber { get; set; }

        [DisplayName("Main Service Category")]
        public string MainServiceCategoryId { get; set; }

        public List<JobClosingDetail> JobClosingDetails { get; set; }
        public List<JobClosingDetailJournal> JournalEntry { get; set; }
    }
}