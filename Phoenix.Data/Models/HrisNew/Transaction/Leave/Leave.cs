using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table(nameof(Leave), Schema = "hr")]
    public class Leave : HrisEntityBase
    {
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("employee_basic_info_id"), StringLength(50)]
        public string EmployeeBasicInfoId { get; set; }

        [Required]
        [Column("start_date")]
        public DateTime StartDate { get; set; }

        [Required]
        [Column("end_date")]
        public DateTime EndDate { get; set; }

        [Required]
        [Column(nameof(Reason)), StringLength(2147483647)]
        public string Reason { get; set; }

        [Column("total_days")]
        public int? TotalDays { get; set; }

        [MaxLength(50)]
        [Column("leave_request_type_id"), StringLength(50)]
        public string LeaveRequestTypeId { get; set; }

        [MaxLength(50)]
        [Column("assisted_job"), StringLength(50)]
        public string AssistedJob { get; set; }

        [MaxLength(50)]
        [Column("other_leave"), StringLength(50)]
        public string OtherLeave { get; set; }

        [MaxLength(100)]
        [Column("file_upload"), StringLength(100)]
        public string FileUpload { get; set; }

        [NotMapped]
        public IFormFile file { get; set; }
        [NotMapped]
        public string FileUploadName { get; set; }

        [MaxLength(100)]
        [Column(nameof(Reached)), StringLength(100)]
        public string Reached { get; set; }

        [Column("request_date")]
        public DateTime? RequestDate { get; set; }

        //public EmployeeBasicInfo EmployeeBasicInfo { get; set; }
        public LeaveRequestType LeaveRequestType { get; set; }

        [NotMapped]
        public InfoEmployee requestEmployee { get; set; }
        [NotMapped]
        public InfoEmployee infoEmployee { get; set; }
        [NotMapped]
        public int? Status { get; set; }
        [NotMapped]
        public string StatusDesc { get; set; }
        [NotMapped]
        public string RemarkRejected { get; set; }
        [NotMapped]
        public string AssistedJobName { get; set; }

        [Column("use_carry_over")]
        public bool? UseCarryOver { get; set; }
        [Column("use_current_year")]
        public bool? UseCurrentYear { get; set; }
    }
}