using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    //table buat menampung transaction id terakhir : yanto
    [Table("transaction_id", Schema = "hr")]
    public class TransactionId
    {
        [Key, Required]
        [MaxLength(50)]
        [Column("trans_id"), StringLength(50)]
        public string TransId { get; set; }

        [Key, Required]
        [Column("request_date")]
        public DateTime RequestDate { get; set; }

        [Column(nameof(Sequence))]
        public int Sequence { get; set; }
    }
}
