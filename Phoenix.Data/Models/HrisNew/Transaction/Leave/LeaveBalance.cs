using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("leave_balance", Schema = "hr")]
    public class LeaveBalance : HrisEntityBase
    {
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("employee_basic_info_id"), StringLength(50)]
        public string EmployeeBasicInfoId { get; set; }

        [Required]
        [Column(nameof(Total))]
        public int Total { get; set; }

        [Required]
        [Column("type")]
        public string Type { get; set; }

        [Column(nameof(Month))]
        public int? Month { get; set; }

        [Required]
        [Column(nameof(Year))]
        public int Year { get; set; }

        [Required]
        [Column("expired_date")]
        public DateTime? ExpiredDate { get; set; }

        [Column(nameof(Balance))]
        public Decimal? Balance { get; set; }

        public EmployeeBasicInfo EmployeeBasicInfo { get; set; }
    }
}