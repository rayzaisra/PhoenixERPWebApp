using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class VwJobCard
    {
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

        public string ClientId { get; set; }

        [DisplayName("Client Name")]
        public string ClientName { get; set; }

        [DisplayName("Client Brand")]
        public string BrandName { get; set; }

        [DisplayName("Campaign Name")]
        public string CampaignName { get; set; }


        [DisplayName("Main Service Category")]
        public string MainserviceCategoryId { get; set; }

        [DisplayName("Share Services")]
        public string ShareservicesId { get; set; }

        [DisplayName("Currency")]
        public string CurrencyId { get; set; }

        [DisplayName("Job Status")]
        public string JobStatus { get; set; }

        [DisplayName("Job Started")]
        public DateTime? JobStarted { get; set; }

        [DisplayName("Business Unit")]
        public string BusinessUnitId { get; set; }

        [DisplayName("Legal Entity")]
        public string LegalEntityId { get; set; }

        [DisplayName("Aging Up Today")]
        public double? AgingUpToDay { get; set; }

        [DisplayName("Type")]
        public string TypeOfExpenseId { get; set; }

        [DisplayName("PCA No")]
        public string PcaId { get; set; }

        [DisplayName("PCE No")]
        public string PceId { get; set; }
        public DateTime? Date { get; set; }
        public int? TotalInvoice { get; set; }
        public int? TotalPO { get; set; }
        //public int? TotalCAUnsettle { get; set; }
        public int? TotalPettyCashUnsettle { get; set; }
        public int? TotalJournalEntryUnsettle { get; set; }
        public int? TotalARUnclosed { get; set; }
        public int? TotalAmountBR { get; set; }
        public int? TotalAmountBP { get; set; }
    }
}