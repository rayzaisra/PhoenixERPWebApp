using DataAccess.PhoenixERP.Finance;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Dto.Custom.Finance
{
    public partial class InventoryGoodsReceiptDetails
    {
        public inventory_goods_receiptDTO good_receipt { get; set; }
        public List<inventory_goods_receipt_detailDTO> good_receipt_details { get; set; }
    }

    public partial class vw_inventory_goods_receipt_detailDTO
    {
        public vw_inventory_goods_receiptDTO good_receipt { get; set; }
    }
}
