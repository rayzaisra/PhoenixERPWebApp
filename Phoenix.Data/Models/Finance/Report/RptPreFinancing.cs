using System;
using System.Collections.Generic;
using System.Text;

namespace Phoenix.Data.Models
{
    public class RptPreFinancing
    {
        public string job_id { get; set; }
        public string job_name { get; set; }
        public decimal? pce_sub_total { get; set; }
        public decimal? pce_asf { get; set; }
        public decimal? pce_vat { get; set; }
        public decimal? pce_grand_total { get; set; }
        public string po_no { get; set; }
        public string po_vendor_name { get; set; }
        public DateTime? po_date { get; set; }
        public decimal? po_amount { get; set; }
        public string ap_no { get; set; }
        public DateTime? ap_date { get; set; }
        public decimal? ap_amount { get; set; }
        public string bp_no { get; set; }
        public DateTime? bp_date { get; set; }
        public decimal? bp_amount { get; set; }
        public decimal? pce_minus_po { get; set; }
        public string ar_no { get; set; }
        public DateTime? ar_date { get; set; }
        public decimal? ar_amount { get; set; }
        public string br_no { get; set; }
        public DateTime? br_date { get; set; }
        public decimal? br_amount { get; set; }
        public decimal? pre_finance { get; set; }
    }

    public class RptPreFinancingFilter
    {
        public string LegalEntityId { get; set; }
        public string BusinessUnitId { get; set; }
        public string CompanyId { get; set; }
        public DateTime? EndingDate { get; set; }
        public string CurrencyId { get; set; }
    }
}
