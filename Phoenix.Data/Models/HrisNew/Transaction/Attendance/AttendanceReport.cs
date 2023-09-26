using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
	public class AttendanceReport
	{
		public string Id { get; set; }

		[DisplayName("DayIn")]
		public string DayIn { get; set; }

		[DisplayName("DateIn")]
		[DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
		public DateTime? DateIn { get; set; }

		[DisplayName("DayOut")]
		public string DayOut { get; set; }

		[DisplayName("DateOut")]
		[DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
		public DateTime? DateOut { get; set; }

		[DisplayName("EarlyIn")]
		public string EarlyIn { get; set; }

		[DisplayName("EarlyOut")]
		public string EarlyOut { get; set; }

		[DisplayName("LateIn")]
		public string LateIn { get; set; }

		[DisplayName("LateOut")]
		public string LateOut { get; set; }

		[DisplayName("Working Hour")]
		public string WorkingHour { get; set; }

		[DisplayName("Total Hour")]
		public string TotalHour { get; set; }

		[DisplayName("Employee Name")]
		public string EmployeeName { get; set; }

	}

	public class AttendanceStatus
	{
        public string EmployeeBasicId { get; set; }
        public bool IsCheckIn { get; set; }
        public TimeSpan? CheckInDateTime { get; set; }
        public bool IsCheckOut { get; set; }
        public TimeSpan? CheckOutDateTime { get; set; }
        public string CheckInLat { get; set; }
        public string CheckInLang { get; set; }
    }
}