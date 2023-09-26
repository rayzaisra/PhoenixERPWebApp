using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class RptBillingAndRevenue : CCYandCCOall
    {
        [DisplayName("Client")]
        public string client_name { get; set; }
    }

    public class RptBillingAndRevenueJobId : CCYandCCOall
    {
        [DisplayName("Job ID-Name")]
        public string job_name { get; set; }
    }

    public class RptBillingAndRevenueSummary : CCYandCCOall
    {
        [DisplayName("Legal Entity")]
        public string legal_entity_name { get; set; }

        [DisplayName("MSC")]
        public string MSC { get; set; }
    }

    public class RptBillingAndRevenueBusinessUnit : CCYandCCOall
    {
        [DisplayName("Business Unit")]
        public string business_unit_name { get; set; }

        [DisplayName("MSC")]
        public string MSC { get; set; }
    }
    public class CCYandCCOall : TotalCloseAndTotalUnclose
    {
        [DisplayName("Revenue")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? ccy_revenue { get; set; }

        [DisplayName("Cost Of Billing")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? ccy_billing { get; set; }

        [DisplayName("Margin/Loss")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? ccy_margin { get; set; }

        [DisplayName("Revenue")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? cco_revenue { get; set; }

        [DisplayName("Cost Of Billing")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? cco_billing { get; set; }

        [DisplayName("Margin/Loss")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? cco_margin { get; set; }
    }

    public class TotalCloseAndTotalUnclose
    {
        [DisplayName("Revenue")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? tc_revenue { get; set; }

        [DisplayName("Cost Of Billing")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? tc_billing { get; set; }

        [DisplayName("Margin/Loss")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? tc_margin { get; set; }

        //[DisplayName("Client Pre Billling")]
        [DisplayName("PCE")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? tu_billing { get; set; }

        //[DisplayName("WIP")]
        [DisplayName("PCA")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? tu_wip { get; set; }

        [DisplayName("Margin/Loss")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? tu_margin { get; set; }

        //[DisplayName("Client Pre Billling")]
        [DisplayName("PCE")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? cco_tu_billing { get; set; }

        //[DisplayName("WIP")]
        [DisplayName("PCA")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? cco_tu_wip { get; set; }

        [DisplayName("Margin/Loss")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? cco_tu_margin { get; set; }
    }

    public class RptBillingAndRevenueFilter
    {

        [DisplayName("Legal Entity")]
        public string LegalEntityId { get; set; }

        [DisplayName("Business Unit")]
        public string BusinessUnitId { get; set; }

        [DisplayName("Client Name")]
        public string ClientId { get; set; }

        [DisplayName("Main Service Category")]
        public string MainServiceCategoryId { get; set; }

        [DisplayName("Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? StartingDate { get; set; }

        [DisplayName("Date")]
        public DateTime? EndingDate { get; set; }

        [DisplayName("Currency")]
        public string CurrencyId { get; set; }

        [DisplayName("Type")]
        public string ReportType { get; set; }

        public string DateType { get; set; }

        [DisplayName("Job ID-Name")]
        public string JobId { get; set; }
    }
}