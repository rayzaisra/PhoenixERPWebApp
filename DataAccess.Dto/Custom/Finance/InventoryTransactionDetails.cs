using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.PhoenixERP.Finance
{
    public partial class InventoryTransactionDetails
    {
        public inventory_transactionDTO transaction { get; set; }
        public List<inventory_transaction_detailDTO> details { get; set; }
    }

    public partial class vw_InventoryTransactionDetails
    {
        public vw_inventory_transactionDTO transaction { get; set; }
        public List<vw_inventory_transaction_detailDTO> details { get; set; }
    }
}
