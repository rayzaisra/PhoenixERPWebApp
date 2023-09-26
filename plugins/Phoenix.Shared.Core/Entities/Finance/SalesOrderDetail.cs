using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("sales_order_detail",Schema ="fn")]
    public partial class SalesOrderDetail : GenericEntity<string>
    {
        public string sales_order_id { get; set; }
    }
}