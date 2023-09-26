using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.PhoenixERP.Finance
{
    public partial class InventoryIn
    {
        public inventory_inDTO transaction { get; set; }
        public List<inventory_in_detailDTO> details { get; set; }
    }
}
