using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class VwJobClosing
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("JOB ID")]
        public string JobId { get; set; }

        [DisplayName("JOB ID")]
        public string JobNumber { get; set; }

        [DisplayName("Job Name")]
        public string JobName { get; set; }

        [DisplayName("Business Unit")]
        public string DivisionName { get; set; }

        [DisplayName("Legal Entity")]
        public string LegalEntityName { get; set; }

        [DisplayName("Client Name")]
        public string ClientName { get; set; }

        [DisplayName("Client Brand")]
        public string BrandName { get; set; }

        [DisplayName("Campaign Name")]
        public string CampaignName { get; set; }

        [DisplayName("Deadline")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Deadline { get; set; }

        [DisplayName("Job Status")]
        public string JobStatus { get; set; }

        [DisplayName("Finance Status")]
        public string FinanceStatus { get; set; }

        [DisplayName("Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? TransactionDate { get; set; }
    }
}