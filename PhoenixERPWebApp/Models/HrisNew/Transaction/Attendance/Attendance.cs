using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class Attendance : HrisEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Employee ID")]
        public string EmployeeBasicInfoId { get; set; }

        [DisplayName("Date Attendance")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateAttendance { get; set; }

        [DisplayName("Time Attendance")]
        [DisplayFormat(DataFormatString = "{0:HH:mm:ss}", ApplyFormatInEditMode = true)]
        public TimeSpan? TimeAttendance { get; set; }

        [DisplayName("Type")]
        public string Type { get; set; }

        [DisplayName("Location Name")]
        public string LocationName { get; set; }

        [DisplayName("Lat")]
        public string Lat { get; set; }

        [DisplayName("Lng")]
        public string Lng { get; set; }

        [DisplayName("Late")]
        public TimeSpan? Late { get; set; }

        [DisplayName("OverTime")]
        public string OverTime { get; set; }

        //[NotMapped]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime ClockIn { get; set; }

        public string LocationIn { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? Clockout { get; set; }

        public string LocationOut { get; set; }


        public InfoEmployee requestEmployee { get; set; }

        //List Attendance
        [DisplayName("Total Hour")]
        //public string TotalHour { get { return (Clockout == null) ? "" : DateTime.Parse(Convert.ToDateTime(Clockout).TimeOfDay.ToString(@"hh\:mm\:ss")).Subtract(DateTime.Parse(ClockIn.TimeOfDay.ToString(@"hh\:mm\:ss"))).ToString(@"hh\:mm"); } }
        public string TotalHour { get; set; }

        [DisplayName("Late")]
        public string LateTime { get { return ((TimeSpan)(Late ?? DateTime.Now.TimeOfDay)).ToString(@"hh\:mm"); } }

        //-----START Report Attendance--------
        [DisplayName("Total Record (data)")]
        public int TotalRecord { get; set; }

        [DisplayName("Month")]
        public string MonthName { get; set; }

        public int Month { get; set; }

        [DisplayName("Year")]
        public int Year { get; set; }
		//-----END Report Attendance--------

		[NotMapped]
		[DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
		public DateTime? DateAttendanceOut { get; set; }

		[DisplayFormat(DataFormatString = "{0:HH:mm:ss}", ApplyFormatInEditMode = true)]
        public TimeSpan? TimeAttendanceOut { get; set; }

		public DateTime? clockinval { get; set; }
		public DateTime? clockoutval { get; set; }

	}
}