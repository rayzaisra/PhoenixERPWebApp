using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class MasterWorkingTime : HrisEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Time In")]
        public TimeSpan? TimeIn { get; set; }

        [DisplayName("Time Out")]
        public TimeSpan? TimeOut { get; set; }

		public bool? IsSunday { get; set; }
		public bool? IsMonday { get; set; }
		public bool? IsTuesday { get; set; }
		public bool? IsWednesday { get; set; }
		public bool? IsThursday { get; set; }
		public bool? IsFriday { get; set; }
		public bool? IsSaturday { get; set; }
	}
}