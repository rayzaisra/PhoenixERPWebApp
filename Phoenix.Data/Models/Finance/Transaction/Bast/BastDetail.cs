using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models.Finance
{
    [Table("inventory_bast_detail", Schema ="fn")]
    public class BastDetail : FinanceEntityBase
    {
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }
        [Column("bast_id")]
        public string BastId { get; set; }
        [Column("good_receipt_id")]
        public string GoodReceiptId { get; set; }

        [Column("service_receipt_id")]
        public string ServiceReceiptId { get; set; }
        [Column("business_unit_id")]
        public string BusinessUnitId { get; set; }
        [Column("legal_entity_id")]
        public string LegalEntityId { get; set; }
        [Column("affiliation_id")]
        public string AffiliationId { get; set; }
        [Column("remarks")]
        public string Remarks { get; set; }
        [Column("date_receipt")]
        public DateTime? DateReceipt { get; set; }
        [Column("item_type")]
        public string ItemType { get; set; }
        [Column("item_name")]
        public string ItemName { get; set; }
        [Column("qty")]
        public decimal? Qty { get; set; }
        [Column("received_qty")]
        public decimal? ReceivedQty { get; set; }
        [Column("remaining_qty")]
        public decimal? RemainingQty { get; set; }
        [Column("location")]
        public string Location { get; set; }
        [Column("inv_fa_id")]
        public string InvFaId { get; set; }
        [Column("bast_type")]
        public string BastType { get; set; }
    }
}
