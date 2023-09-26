using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("leave_request_type", Schema = "hr")]
    public class LeaveRequestType : HrisEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Column(nameof(Name)), StringLength(50)]
        public string Name { get; set; }

        [Column("is_reduce_balance")]
        public bool IsReduceBalance { get; set; }

        [Column("is_paid_leave")]
        public bool IsPaidLeave { get; set; }

        [Column("minimum_work")]
        public int MinimumWork { get; set; }

        [Column("minimum_duration_leave")]
        public int MinimumDurationLeave { get; set; }

        [Column("maximum_duration_leave")]
        public int MaximumDurationLeave { get; set; }
        [Column("leave_duration_in_a_year")]
        public int LeaveDurationInAYear { get; set; }

        [Column("is_carry_over")]
        public bool? IsCarryOver { get; set; }
        [Column("maximum_days_in_a_month")]
        public int MaximumDaysInAMonth { get; set; }
        [Column("grade_level_employee_id")]
        public string GradeLevelEmployeeId { get; set; }
        [NotMapped]
        public string GradeLevelEmployeeName { get; set; }
    }
}

