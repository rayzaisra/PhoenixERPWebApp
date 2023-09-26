using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
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
}