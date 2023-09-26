using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models
{
    [Table("payment_request_detail", Schema = "fn")]
    public class PaymentRequestDetail : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("payment_request_id"), StringLength(50)]
        public string PaymentRequestId { get; set; }

        [Column("currency_id")]
        public string CurrencyId { get; set; }

        [Column("qty")]
        public decimal? Qty { get; set; }

        [Column("pay_unit_price")]
        public decimal? PayUnitPrice { get; set; }

        [MaxLength(50)]
        [Column("referenceId"), StringLength(50)]
        public string ReferenceId { get; set; }

        [MaxLength(50)]
        [Column("accountId"), StringLength(50)]
        public string AccountId { get; set; }

        [MaxLength(50)]
        [Column("job_id"), StringLength(50)]
        public string JobId { get; set; }

        [Column("vat")]
        public decimal? Vat { get; set; }

        [Column("tax_payable")]
        public decimal? TaxPayable { get; set; }

        [Column("source_doc")]
        public string SouceDoc { get; set; }

        [Column("pairing_group_id")]
        public string PairingGroupId { get; set; }

        [Column("tax_payable_coa_id")]
        public string TaxPayableCoaId { get; set; }

        [NotMapped]
        public string TaxPayableCoaName { get; set; }


        [NotMapped]
        public string APNumber { get; set; }

        [NotMapped]
        public string JobName { get; set; }
    }
}
