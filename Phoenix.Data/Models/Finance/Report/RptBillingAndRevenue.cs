using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    public class RptBillingAndRevenue : CCYandCCOall
    {
        public string client_name { get; set; }
    }

    public class RptBillingAndRevenueJobId : CCYandCCOall
    {
        public string job_name { get; set; }
    }

    public class RptBillingAndRevenueSummary : CCYandCCOall
    {
        public string legal_entity_name { get; set; }
        public string MSC { get; set; }
    }

    public class RptBillingAndRevenueBusinessUnit : CCYandCCOall
    {
        public string business_unit_name { get; set; }
        public string MSC { get; set; }
    }

    public class CCYandCCOall : TotalCloseAndTotalUnclose
    {
        public decimal? ccy_revenue { get; set; }
        public decimal? ccy_billing { get; set; }
        public decimal? ccy_margin { get; set; }
        public decimal? cco_revenue { get; set; }
        public decimal? cco_billing { get; set; }
        public decimal? cco_margin { get; set; }
    }

    public class TotalCloseAndTotalUnclose
    {
        public decimal? tc_revenue { get; set; }
        public decimal? tc_billing { get; set; }
        public decimal? tc_margin { get; set; }
        public decimal? tu_billing { get; set; }
        public decimal? tu_wip { get; set; }
        public decimal? tu_margin { get; set; }
        public decimal? cco_tu_billing { get; set; }
        public decimal? cco_tu_wip { get; set; }
        public decimal? cco_tu_margin { get; set; }
    }

    public class RptBillingAndRevenueFilter
    {
        public string LegalEntityId { get; set; }
        public string BusinessUnitId { get; set; }
        public string ClientId { get; set; }
        public string MainServiceCategoryId { get; set; }
        public DateTime? StartingDate { get; set; }
        public DateTime? EndingDate { get; set; }
        public string CurrencyId { get; set; }
        public string ReportType { get; set; }
        public string DateType { get; set; }
        public string JobId { get; set; }
    }
}