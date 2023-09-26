using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models
{
    [Table("invoice_client_item_other", Schema = "fn")]
    public class InvoiceClientItemOther
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("parent_id"), StringLength(50)]
        public string ParentId { get; set; }

        [MaxLength(50)]
        [Column("invoice_id"), StringLength(50)]
        public string InvoiceId { get; set; }

        //[MaxLength(250)]
        //[Column(nameof(Description)), StringLength(250)]
        //public string Description { get; set; }

        //[MaxLength(250)]
        //[Column(nameof(Remark)), StringLength(250)]
        //public string Remark { get; set; }

        [Required]
        [Column(nameof(Amount))]
        public decimal Amount { get; set; }

        [Column("sequence")]
        public int? Sequence { get; set; }

        [Column("account_id")]
        public string AccountID { get; set; }

        [Column("pce_id")]
        public string PceID { get; set; }

        [Column("job_id")]
        public string JobID { get; set; }

        [MaxLength(50)]
        [Column("task_id"), StringLength(50)]
        public string TaskId { get; set; }

        [Column("amount_percentage")]
        public decimal AmountPercentage { get; set; }

        [Column("quantity_other")]
        public int? Quantity { get; set; }

        [MaxLength(50)]
        [Column("task_name_other"), StringLength(50)]
        public string TaskName { get; set; }

        [Column("unit_price_other")]
        public decimal? UnitPrice { get; set; }

        [Column("amount_billed_other")]
        public decimal? AmountBilled { get; set; }

        [NotMapped]
        public decimal? Balance { get; set; }

        [NotMapped]
        public bool? IsAsf { get; set; }

        [NotMapped]
        public string CurrencySymbol { get; set; }
    }
}
