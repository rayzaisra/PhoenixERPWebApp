using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("request_for_quotation", Schema ="fn")]
    public partial class RequestForQuotation : GenericEntity<string>
    {
        public string internal_rfq_id { get; set; }
        public string request_for_quotation_number { get; set; }
        public DateTime? request_for_quotation_date { get; set; }
        public string legal_entity_id { get; set; }
        public string business_unit_id { get; set; }
        public string affiliation_id { get; set; }
        public string brand_name { get; set; }
        public string currency_id { get; set; }
        public decimal? exchange_rate { get; set; }
        public string payment_type_id { get; set; }
        public string delivery_place { get; set; }
        public DateTime? delivery_date { get; set; }
        public DateTime? closing_date { get; set; }
        public string term_of_payment { get; set; }
        public string job_id { get; set; }
        public string job_pa_id { get; set; }
        public string job_pe_id { get; set; }
        public string rfq_type_id { get; set; }
        public bool? is_prefered_vendor { get; set; }
        public string approval_id { get; set; }
        public string purchase_request_id { get; set; }
    }
    public class RfqCode : GenericEntity<string>
    {

    }
}