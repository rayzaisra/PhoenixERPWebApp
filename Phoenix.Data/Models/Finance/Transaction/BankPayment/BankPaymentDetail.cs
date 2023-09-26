using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models.Finance.Transaction.BankPayment
{
    [Table("bank_payment_detail", Schema = "fn")]
    public class BankPaymentDetail : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [Column("bank_payment_id")]
        public string BankPaymentId { get; set; }

        [Column("account_id")]
        public string AccountId { get; set; }

        [Column("debit")]
        public decimal? Debit { get; set; }

        [Column("credit")]
        public decimal? Credit { get; set; }
    }
}
