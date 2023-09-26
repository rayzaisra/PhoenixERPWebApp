using System;
using System.Collections.Generic;
using System.Text;

namespace Phoenix.Data.Models.Finance
{
    public class InventoryDTO
    {
        public Inventory Header { get; set; }
        public List<InventoryDetail> InventoryDetails { get; set; }
    }
}
