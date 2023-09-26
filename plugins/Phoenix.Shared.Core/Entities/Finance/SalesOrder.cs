using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("sales_order",Schema ="fn")]
    public partial class SalesOrder : GenericEntity<string>
    {
        public string business_unit_id { get; set; }
        public string company_id { get; set; }
        public string company_reference { get; set; }
        public string description { get; set; }
        public string organization_id { get; set; }
        public string sales_order_number { get; set; }
    }
}