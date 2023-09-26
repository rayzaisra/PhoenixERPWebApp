using DataAccess.PhoenixERP.Finance;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Dto.Custom.Finance
{
    public partial class PurchaseOrderDetails
    {
        public string approverId { get; set; }
        public purchase_orderDTO purchase_order { get; set; }
        public List<purchase_order_detailDTO> purchase_order_details { get; set; }

    }

    public partial class vw_purchase_orderDetailsDTO
    {
        public string approverId { get; set; }
        public vw_purchase_orderDTO purchase_order { get; set; }
    }
}
