using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models.Finance
{
    [Table("petty_cash_detail", Schema = "fn")]
    public class PettyCashDetail : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("petty_cash_id"), StringLength(50)]
        public string PettyCashId { get; set; }

        [MaxLength(50)]
        [Column("account_id"), StringLength(50)]
        public string AccountId { get; set; }

        [Column("transaction_date")]
        public DateTime? TransactionDate { get; set; }

        [MaxLength(450)]
        [Column("transaction_detail"), StringLength(450)]
        public string TransactionDetail { get; set; }

        [Column(nameof(Credit))]
        public decimal? Credit { get; set; }

        [MaxLength(50)]
        [Column("job_id"), StringLength(50)]
        public string JobId { get; set; }

        [MaxLength(50)]
        [Column("pce_id"), StringLength(50)]
        public string PceId { get; set; }

        [MaxLength(50)]
        [Column("pca_id"), StringLength(50)]
        public string PcaId { get; set; }

        [MaxLength(50)]
        [Column("pca_task_id"), StringLength(50)]
        public string PcaTaskId { get; set; }

        [MaxLength(50)]
        [Column("business_unit_id"), StringLength(50)]
        public string BusinessUnitId { get; set; }

        [NotMapped]
        public string JobName { get; set; }
         
        [NotMapped]
        public string PcaTask { get; set; }

        [NotMapped]
        public string AccountName { get; set; }

        [NotMapped]
        public string BusinessUnitName { get; set; }
    }
}
