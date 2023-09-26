using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("salary_payment_request", Schema = "fn")]
    public class SalaryPaymentRequest : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("legal_entity_id"), StringLength(50)]
        public string LegalEntityId { get; set; }

        [MaxLength(50)]
        [Column("payroll_id"), StringLength(50)]
        public string PayrollId { get; set; }

        [MaxLength(255)]
        [Column(nameof(Description)), StringLength(255)]
        public string Description { get; set; }

        [Column("salary_payment_date")]
        public DateTime? SalaryPaymentDate { get; set; }

        [Column("transaction_date")]
        public DateTime? TransactionDate { get; set; }

        [MaxLength(50)]
        [Column("currency_id"), StringLength(50)]
        public string CurrencyId { get; set; }

        [Column("exchange_rate")]
        public decimal? ExchangeRate { get; set; }

        [Column("total_amount")]
        public decimal? TotalAmount { get; set; }

        [MaxLength(50)]
        [Column(nameof(Status)), StringLength(50)]
        public string Status { get; set; }

        //For List
        [NotMapped]
        public string LegalEntityName { get; set; }

        [NotMapped]
        public string RequestBy { get; set; }

        //For Approval 
        [NotMapped]
        public string LogDescription { get; set; }

        [NotMapped]
        public List<SalaryPaymentRequestDetail> Details { get; set; }

        [NotMapped]
        public string CurrencySymbol { get; set; }
    }
}