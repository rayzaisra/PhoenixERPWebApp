using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models.Finance.Transaction.ServiceReceipt
{
    [Table("service_receipt_detail", Schema = "fn")]
    public class ServiceReceiptDetail : FinanceEntityBase
    {
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }
        [Column("service_receipt_id")]//,<, nvarchar(50),>
        public string ServiceReceiptId { get; set; }
        [Column("item_id")]   //,<, nvarchar(50),>
        public string ItemId { get; set; }
        [Column("item_name")]   // ,<, nvarchar(450),>
        public string ItemName { get; set; }
        [Column("item_type_id")]   //,<, nvarchar(50),>
        public string ItemTypeId { get; set; }
        [Column("item_type_name")]   // ,<, nvarchar(450),>
        public string ItemTypeName { get; set; }
        [Column("item_category_id")]  // ,<, nvarchar(50),>
        public string CategoryId { get; set; }
        [Column("item_category_name")]   //,<, nvarchar(450),>
        public string CategoryName { get; set; }
        [Column("uom_id")]  // ,<, nvarchar(50),>
        public string UOMId { get; set; }
        [Column("unit_name")]  //,<, nvarchar(450),>
        public string UnitName { get; set; }
        [Column("brand")]  // ,<, nvarchar(450),>
        public string Brannd { get; set; }
        [Column("subbrand")]  // ,<, nvarchar(450),>
        public string SubBrand { get; set; }
        [Column("description")]  // ,<, nvarchar(450),>
        public string Description { get; set; }
        [Column("remark")]   // ,<, nvarchar(450),>
        public string Remark { get; set; }
        [Column("file_attachment")] //  ,<, nvarchar(50),>
        public string FileAttachment { get; set; }
        [Column("purchase_order_detail_id")] // ,<, nvarchar(50),>
        public string PurchaseOrderDetailId { get; set; }
        [Column("item_code")]   //,<, nvarchar(50),>)
        public string ItemCode { get; set; }
    }
}
