using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("quotation_vendor",Schema ="pm")]
    public partial class QuotationVendor : GenericEntity<string>
    {
        public string delivery_place { get; set; }
        public DateTime? delivery_time { get; set; }
        public Single? downpayment { get; set; }
        public DateTime? expiry_date { get; set; }
        public string external_id { get; set; }
        public DateTime? lead_time { get; set; }
        public string noted { get; set; }
        public string payment_id { get; set; }
        public string pe_bidding_id { get; set; }
        public Single? price { get; set; }
        public int? qty { get; set; }
        public DateTime? quotation_date { get; set; }
        public string rfq_number { get; set; }
        public string task_id { get; set; }
        public string transaction_type_id { get; set; }
    }
}