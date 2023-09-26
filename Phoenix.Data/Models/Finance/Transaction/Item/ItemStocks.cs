using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models.Finance.Transaction.Item
{
    [Table("item_stock")]
    public class ItemStocks :FinanceEntityBase
    {
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }
        [Column("item_id")]  //,<, nvarchar(50),>
        public string ItemId { get; set; }
        [Column("warehouse_id")]  // ,<, nvarchar(50),>
        public string WarehouseId { get; set; }
        [Column("uom_id")]  //,<, nvarchar(50),>
        public string UOMId { get; set; }
        [Column("qty")]  // ,<, decimal (18,5),>)
        public decimal Qty { get; set; }
    }
}
