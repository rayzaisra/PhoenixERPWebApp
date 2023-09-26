using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("salary_payment_request_detail", Schema = "fn")]
    public class SalaryPaymentRequestDetail : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("salary_payment_request_id"), StringLength(50)]
        public string SalaryPaymentRequestId { get; set; }

        [MaxLength(50)]
        [Column("division_id"), StringLength(50)]
        public string DivisionId { get; set; }

        [MaxLength(250)]
        [Column("division_name"), StringLength(250)]
        public string DivisionName { get; set; }

        [Column(nameof(Amount))]
        public decimal? Amount { get; set; }
    }
}