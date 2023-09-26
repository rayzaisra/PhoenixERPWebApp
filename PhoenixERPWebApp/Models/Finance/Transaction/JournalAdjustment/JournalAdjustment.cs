using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class JournalAdjustment : FinanceEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Reference No")]
        public string Code { get; set; }

        [DisplayName("Date Transaction")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? JvDate { get; set; }

        [DisplayName("Type")]
        public string Type { get; set; }

        [DisplayName("Currency")]
        public string CurrencyId { get; set; }

        [DisplayName("Exchange Rate")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? ExchangeRate { get; set; }

        [DisplayName("Reference Id")]
        public string ReferenceId { get; set; }

        [DisplayName("Reference No")]
        public string JvReferenceNo { get; set; }

        [DisplayName("Description")]
        public string JvDescription { get; set; }

        [DisplayName("Legal Entity")]
        public string LegalEntityId { get; set; }

        [DisplayName("Business Unit")]
        public string BusinessUnitId { get; set; }

        [DisplayName("Affiliation")]
        public string AffiliationId { get; set; }

        [DisplayName("Main Service Category")]
        public string MainServiceCategoryId { get; set; }

        [DisplayName("JOB ID")]
        public string JobId { get; set; }

        [DisplayName("PCE ID")]
        public string PceId { get; set; }

        [DisplayName("PCA ID")]
        public string PcaId { get; set; }

        [DisplayName("Client Name")]
        public string ClientId { get; set; }

        [DisplayName("Fiscal Year")]
        public string FinancialYearId { get; set; }

        [DisplayName("Period")]
        public string FinancePeriodId { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; }

        //Not Mapped
        public List<JournalAdjustmentDetail> JournalAdjustmentDetails { get; set; }

        [DisplayName("Legal Entity")]
        public string LegalEntityName { get; set; }

        [DisplayName("Business Unit")]
        public string BusinessUnitName { get; set; }

        [DisplayName("Afiliation")]
        public string AffiliationName { get; set; }

        [DisplayName("Client Name")]
        public string ClientName { get; set; }

        [DisplayName("Main Service Category")]
        public string MainServiceCategoryName { get; set; }

        [DisplayName("Fiscal Year")]
        public string ViewFiscalYear { get; set; }

        [DisplayName("Period")]
        public string ViewPeriod { get; set; }

        [DisplayName("Date Transaction")]
        public DateTime? ViewDateTransaction { get; set; }

        //For List Search
        public DateTime? JvDateEnd { get; set; }

        //For Approval
        public int StatusApproval { get; set; }

        [DisplayName("Remarks")]
        [MaxLength(255)]
        public string RemarksProcess { get; set; }
    }

    public class JournalAdjustmentSearchNumber
    {
        public string Type { get; set; }
        public int? FiscalYear { get; set; }
        public DateTime? StartPeriod { get; set; }
        public DateTime? EndPeriod { get; set; }
    }
}