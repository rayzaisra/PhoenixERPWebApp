using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table(nameof(Attendance), Schema = "hr")]
    public class Attendance : HrisEntityBase
    {
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("employee_basic_info_id"), StringLength(50)]
        public string EmployeeBasicInfoId { get; set; }

        [Column("date_attendance")]
        public DateTime? DateAttendance { get; set; }

        [Column("time_attendance")]
        public TimeSpan? TimeAttendance { get; set; }

        [MaxLength(10)]
        [Column(nameof(Type)), StringLength(10)]
        public string Type { get; set; }

        [MaxLength(100)]
        [Column("location_name"), StringLength(100)]
        public string LocationName { get; set; }

        [MaxLength(30)]
        [Column(nameof(Lat)), StringLength(30)]
        public string Lat { get; set; }

        [MaxLength(30)]
        [Column(nameof(Lng)), StringLength(30)]
        public string Lng { get; set; }

        [MaxLength(10)]
        [DisplayName("late")]
        public TimeSpan? Late { get; set; }

        [MaxLength(10)]
        [DisplayName("overtime")]
        public string OverTime { get; set; }

        [NotMapped]
        public InfoEmployee requestEmployee { get; set; }

		[Column("date_attendance_out")]
		public DateTime? DateAttendanceOut { get; set; }

		[Column("time_attendance_out")]
		public TimeSpan? TimeAttendanceOut { get; set; }

		//-----START Report Attendance--------
		[NotMapped]
        public int TotalRecord { get; set; }

        [NotMapped]
        public string MonthName { get; set; }

        [NotMapped]
        public int Month { get; set; }

        [NotMapped]
        public int Year { get; set; }
		//-----END Report Attendance--------		

		[NotMapped]
		public DateTime? clockinval { get; set; }

		[NotMapped]
		public DateTime? clockoutval { get; set; }

        [NotMapped]
        public string TotalHour { get; set; }
    }
}