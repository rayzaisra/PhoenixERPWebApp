using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("purchase_order_detail",Schema ="fn")]
    public partial class PurchaseOrderDetail : GenericEntity<string>
    {
        public string purchase_order_id { get; set; }
    }
}