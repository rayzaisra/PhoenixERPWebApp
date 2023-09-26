using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("mass_leave", Schema = "hr")]
    public class MassLeave : HrisEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [Required]
        [Column(nameof(Year))]
        public int Year { get; set; }

        [Required]
        [Column("leave_from")]
        public DateTime LeaveFrom { get; set; }

        [Required]
        [Column("leave_to")]
        public DateTime LeaveTo { get; set; }

        [Required]
        [Column("total_days")]
        public int TotalDays { get; set; }

        [Column(nameof(Reason)), StringLength(2147483647)]
        public string Reason { get; set; }

        [MaxLength(50)]
        [Column("request_by"), StringLength(50)]
        public string RequestBy { get; set; }

        [Column("request_date")]
        public DateTime? RequestDate { get; set; }

        [NotMapped]

        public List<MassLeaveDetail> MassLeaveDetails { get; set; }

        [NotMapped]
        public int? Status { get; set; }

        [NotMapped]
        public string StatusDesc { get; set; }

        [NotMapped]
        public string RemarkRejected { get; set; }
        [NotMapped]
        public InfoEmployee requestEmployee { get; set; }
    }
}