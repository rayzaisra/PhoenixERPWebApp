using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("employee_loan_paid", Schema = "hr")]
    public class EmployeeLoanPaid : HrisEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("employee_loan_id"), StringLength(50)]
        public string EmployeeLoanId { get; set; }

        [MaxLength(50)]
        [Column("employee_basic_info_id"), StringLength(50)]
        public string EmployeeBasicInfoId { get; set; }

        [Column("log")]
        public string Log { get; set; }

        [Column("date_paid")]
        public DateTime? DatePaid { get; set; }

        [Column(nameof(Value))]
        public decimal? Value { get; set; }

        [Column(nameof(Status))]
        public int? Status { get; set; }

        [Column("early_paid_date")]
        public DateTime? EarlyPaidDate { get; set; }

        [NotMapped]
        public string DatePaidString { get; set; }
        [NotMapped]
        public decimal? ValuePaid { get; set; }
    }
}