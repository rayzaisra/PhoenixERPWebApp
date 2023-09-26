using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("request_for_quotation_detail", Schema ="fn")]
    public partial class RequestForQuotationDetail : GenericEntity<Guid>
    {
        public string request_for_quotation_id { get; set; }
        public string item_id { get; set; }
        public string item_name { get; set; }
        public string item_type { get; set; }
        public string uom_id { get; set; }
        public string subbrand_name { get; set; }
        public string description { get; set; }
        public decimal qty { get; set; }
        public int? qty_shift { get; set; }
        public int? qty_day { get; set; }
        public int? qty_week { get; set; }
        public int? qty_city { get; set; }
        public decimal unit_price { get; set; }
        public string currency_id { get; set; }
        public decimal exchange_rate { get; set; }
        public decimal? amount { get; set; }
        public string category_id { get; set; }
        public string filemaster_id { get; set; }
        public string task { get; set; }
        public string subtask { get; set; }
        public int revision_status_pca { get; set; }
        public string detail_spec_file { get; set; }
    }
}