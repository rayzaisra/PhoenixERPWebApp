using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("purchase_order",Schema ="fn")]
    public partial class PurchaseOrder : GenericEntity<string>
    {
        public string business_unit_id { get; set; }
        public string company_id { get; set; }
        public string company_reference { get; set; }
        public string description { get; set; }
        public string legal_entity_id { get; set; }
        public string organization_id { get; set; }
        public string purchase_order_number { get; set; }
    }
}