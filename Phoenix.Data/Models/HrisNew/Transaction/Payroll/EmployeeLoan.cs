using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{

    [Table("employee_loan", Schema = "hr")]
    public class EmployeeLoan : HrisEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("employee_basic_info_id"), StringLength(50)]
        public string EmployeeBasicInfoId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("loan_category_id"), StringLength(50)]
        public string LoanCategoryId { get; set; }

        [Required]
        [Column("date_loan")]
        public DateTime DateLoan { get; set; }

        [Column(nameof(Value))]
        public decimal? Value { get; set; }

        [Required]
        [Column("payroll_deduction")]
        public int PayrollDeduction { get; set; }

        [Required]
        [Column("repayment_start_date")]
        public DateTime RepaymentStartDate { get; set; }

        //public LoanCategory LoanCategory { get; set; }
        [NotMapped]
        public InfoEmployee infoEmployee { get; set; }
        [NotMapped]
        public string LoanCategoryName { get; set; }
        [NotMapped]
        public decimal InterestValue { get; set; }
        [NotMapped]
        public decimal InterestMonth { get; set; }
        [NotMapped]
        public decimal? Remaining { get; set; }
        [NotMapped]
        public bool isPaid { get; set; }

        [Column("value_total_installment")]
        public decimal? ValueTotalInstallment { get; set; }

        [Column("value_installment_amount")]
        public decimal? ValueInstallmentAmount { get; set; }

        [Column("value_interest_amount")]
        public decimal? ValueInterestAmount { get; set; }
        [Column("status")]
        public int? Status { get; set; }

        [Column("log_description")]
        public string LogDescription { get; set; }
        [NotMapped]
        public int? StatusApproval { get; set; }

        [NotMapped]
        public bool IsCorporateLoan { get; set; }

        [NotMapped]
        public string StatusDesc { get; set; }


    }
}