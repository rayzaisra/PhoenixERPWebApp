using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("quotation_request",Schema ="pm")]
    public partial class QuotationRequest : GenericEntity<string>
    {
        public DateTime? closing_date { get; set; }
        public string company_id { get; set; }
        public int? days { get; set; }
        public DateTime? delivery_date { get; set; }
        public string delivery_place { get; set; }
        public DateTime? fq_date { get; set; }
        public string payment_id { get; set; }
        public int? remarks { get; set; }
        public int? shift { get; set; }
        public string task_id { get; set; }
        public string transaction_type_id { get; set; }
        public int? weeks { get; set; }
    }
}